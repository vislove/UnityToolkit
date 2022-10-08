using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResetCore.Util
{
    /// <summary>
    /// 任务管理类
    /// </summary>
    public class TaskManager
    {
        public Task StartTask(MonoBehaviour mono,IEnumerator enumerator, Action action = null, bool isAutoStart = true)
        {
            var r = new Task(mono, enumerator);
            r.Action = action;
            if (isAutoStart) r.Start();
            return r;
        }
    }

    public class Task
    {
        public Task(MonoBehaviour mono, IEnumerator coroutine)
        {
           this.coroutine = coroutine;
           this.mono = mono;
        }

        private IEnumerator coroutine;
        private MonoBehaviour mono;
        private bool running;
        private bool paused;
        private bool stopped;
        private Action action;
        public bool Running { get { return running; } }
        public bool Paused { get { return paused; } }
        public bool Stopped { get { return stopped; } }
        public Action Action { get { return action; } set { action = value; } }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public void Start()
        {
            running = true;
            if (mono != null)
            {
                mono.StartCoroutine(CallWrapper());
            }
            else
            {
                Debug.LogErrorFormat("Task Exception!!! {0} is null", mono);
            }
        }

        public void Stop()
        {
            stopped = true;
            running = false;
        }

        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = coroutine;
            while (running)
            {
                if (paused)
                {
                    yield return null;
                }
                else
                {
                    if (e != null && e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    else
                    {
                        running = false;
                    }
                }
            }
            
            if (Action != null) Action();
        }
    }
}
