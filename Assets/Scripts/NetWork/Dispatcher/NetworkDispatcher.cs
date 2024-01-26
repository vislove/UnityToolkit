using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网络派发器
/// </summary>
public class NetworkDispatcher
{
    private static NetworkDispatcher _ins;
    private static readonly object _observerSync = new object();
    private IDictionary<string, List<INetDispatcherObject>> _observerMap = null;
    private IList<INetDispatcherObject> _permanentList = null;
    private Queue<(string, INetDispatcherObject)> _removeObjectQueue = null;


    public static NetworkDispatcher Instance
    {
        get
        {
            if (null == _ins)
            {
                _ins = new NetworkDispatcher();
            }

            return _ins;
        }
    }

    private NetworkDispatcher()
    {
        _permanentList = new List<INetDispatcherObject>();
        _observerMap = new Dictionary<string, List<INetDispatcherObject>>();
        _removeObjectQueue = new Queue<(string, INetDispatcherObject)>();
    }
    
     /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="observerName"></param>
    public void AddListener(string opCode, INetDispatcherObject dispatcherObject, bool markAsPermanent = true)
    {
        lock (_observerSync)
        {
            if (null != _observerMap)
            {
                if (markAsPermanent)
                {
                    _permanentList.Add(dispatcherObject);
                }

                if (_observerMap.TryGetValue(opCode, out var dispatcherObjects))
                {
                    dispatcherObjects.Add(dispatcherObject);
                }
                else
                {
                    dispatcherObjects = new List<INetDispatcherObject>() { dispatcherObject };
                    _observerMap.Add(opCode, dispatcherObjects);
                }
            }
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="observerName"></param>
    public void RemoveListener(string opCode)
    {
        lock (_observerSync)
        {
            if (_observerMap.TryGetValue(opCode, out var dispatcherObjects))
            {
                for (int iLoop = 0; iLoop < dispatcherObjects.Count; iLoop++)
                {
                    if (_permanentList.Contains(dispatcherObjects[iLoop]))
                    {
                        _permanentList.Remove(dispatcherObjects[iLoop]);
                    }
                }
                _observerMap.Remove(opCode);
            }
        }
    }

    /// <summary>
    /// 移除监听对象
    /// </summary>
    public void RemoveListenerObject(string opCode, INetDispatcherObject dispatcherObject)
    {
        lock (_observerSync)
        {
            if (!_removeObjectQueue.Contains((opCode, dispatcherObject)))
            {
                _removeObjectQueue.Enqueue((opCode, dispatcherObject));
            }
            else
            {
                Debug.LogError("已经添加过被移除的监听对象了");
            }
        }
    }

    /// <summary>
    /// 通知监听者消息
    /// </summary>
    /// <param name="netOpCode"></param>
    /// <param name="content"></param>
    public void Dispatch<T>(string netOpCode, T args)
    {
        lock (_observerSync)
        {
            if (null != _observerMap)
            {
                if (_observerMap.TryGetValue(netOpCode, out var dispatcherObjects))
                {
                    var temp = dispatcherObjects.GetEnumerator();
                    while (temp.MoveNext())
                    {
                        try
                        {
                            if (!_removeObjectQueue.Contains((netOpCode, temp.Current)))
                            {
                                temp.Current?.HandleNetWorkNotice(netOpCode, args);
                                if (!_permanentList.Contains(temp.Current)) // 如果不是持久对象，只派发一次
                                {
                                    Debug.Log($"=====不是持久性派发数据，移除监听 {temp.Current}");
                                    RemoveListenerObject(netOpCode, temp.Current);
                                }
                            }
                            else
                            {
                                Debug.Log("包含移除对象，不进行消息派发！");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"NetworkDispatcher Exception: {ex}");
                        }
                    }

                    HandleRemoveListenerObject();
                }
            }
        }
    }
    
    public void Dispose()
    {
        _observerMap = null;
        _removeObjectQueue = null;
        _permanentList = null;
    }

    /// <summary>
    /// 处理移除监听对象
    /// </summary>
    private void HandleRemoveListenerObject()
    {
        for (int iLoop = 0; iLoop < _removeObjectQueue.Count; iLoop++)
        {
            var item = _removeObjectQueue.Dequeue();
            if (_permanentList.Contains(item.Item2)) // 移除持久化对象
            {
                _permanentList.Remove(item.Item2);
            }

            if (_observerMap.TryGetValue(item.Item1, out var dispatcherObjects))
            {
                dispatcherObjects.Remove(item.Item2);
            }
        }
    }
}