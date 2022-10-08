using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 基于Mono的单利扩展.
/// 例子: 
/// <code>
/// public class Foo : MonoSingleton<Foo>
/// </code>
/// 获取Foo的实例, use <code>Foo.instance</code>
/// 注意：1.继承MonoSingleton的类需要重写<code> Init()</code>方法来初始化而不是使用Awake()
/// 2.继承MonoSingleton的类在Unity的声明周期中，Init()等价于Awake()所以还是按照 Awake() -> Start()顺序执行...
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;

    public static T Instance
    {
        get
        {
            // 第一次调用该实例时查找该实例是否被绑定在GameObject，如果没有则创建一个临时的GameObject将其绑定，并设置跳转场景不销毁该对象
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                // 如果没有找到则创建一个空对象
                if (m_Instance == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                    isTemporaryInstance = true;
                    m_Instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    // 正常情况下创建的物体是不为空的，这里只是做一下容错
                    if (m_Instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }

                }
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    m_Instance.Init();
                }
            }
            return m_Instance;
        }
        protected set
        {
            m_Instance = value;
        }
    }

    public static bool isTemporaryInstance { private set; get; }

    private static bool _isInitialized;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
        else if (m_Instance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(this);
            return;
        }
        if (!_isInitialized)
        {
            DontDestroyOnLoad(gameObject);
            _isInitialized = true;
            m_Instance.Init();
        }
    }

    /// <summary>
    /// 如果想在Awake()进行初始化那么必须重写Init(),用Init()代替Awake()
    /// </summary>
    public virtual void Init()
    {
    }

    /// <summary>
    /// 当退出应用时将实例置空
    /// </summary>
    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}


