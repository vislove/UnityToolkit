﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;
namespace ResetCore.Util
{
    /// <summary>
    /// 线程辅助类
    /// </summary>
    public class ThreadHelper : MonoBehaviour
    {
        public static int maxThreads = 8;
        static int numThreads;

        private static ThreadHelper _current;
        public static ThreadHelper Current
        {
            get
            {
                Initialize();
                return _current;
            }
        }

        void Awake()
        {
            _current = this;
            initialized = true;
        }

        static bool initialized;

        static void Initialize()
        {
            if (!initialized)
            {

                if (!Application.isPlaying)
                    return;
                initialized = true;
                var g = new GameObject("TheardHelper");
                _current = g.AddComponent<ThreadHelper>();
            }
        }

        private List<Action> _actions = new List<Action>();
        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        /// <summary>
        /// 在主线程上执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="waitForSeconds"></param>
        public static void RunOnMainThread(Action action, float waitForSeconds = 0)
        {
            QueueOnMainThread(action, waitForSeconds);
        }
        public static void QueueOnMainThread(Action action, float time)
        {
            if (time != 0)
            {
                lock (Current._delayed)
                {
                    Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (Current._actions)
                {
                    Current._actions.Add(action);
                }
            }
        }

        /// <summary>
        /// 在子线程中执行
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Thread RunOnAsync(Action a)
        {
            Initialize();
            //线程过多
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }
            //以原子操作的模式递增：numThreads++
            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, a);
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
            }
            finally
            {
                //以原子操作的模式递减：numThreads--
                Interlocked.Decrement(ref numThreads);
            }

        }


        void OnDisable()
        {
            if (_current == this)
            {

                _current = null;
            }
        }

        List<Action> _currentActions = new List<Action>();

        // 在主线程上执行
        void Update()
        {
            //加入动作
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            //遍历动作并且执行
            foreach (var a in _currentActions)
            {
                a();
            }

            //加入延时动作
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                foreach (var item in _currentDelayed)
                    _delayed.Remove(item);
            }

            //遍历并且执行
            foreach (var delayed in _currentDelayed)
            {
                delayed.action();
            }
        }
    }
}