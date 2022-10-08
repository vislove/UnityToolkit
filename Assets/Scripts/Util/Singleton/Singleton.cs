using System;
/// <summary>
/// 单利
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : Singleton<T>, new()
{

    private static T _Instance = null;

    protected virtual bool autoInit
    {
        get { return true; }
    }

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                if (_Instance.autoInit)
                    _Instance.Init();
            }
            return _Instance;
        }
    }

    public virtual void Init() { }
}

