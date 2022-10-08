using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnityToAndroid交互类(使用完自动释放)
/// </summary>
public class UnityToAndroid
{
    /// <summary>
    /// Unity调用安卓方法无返回值
    /// </summary>
    /// <param name="aMethodName"></param>
    /// <param name="content"></param>
    public static void Call(string aMethodName, params object[] content)
    {
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jo.Call(aMethodName, content);
            }
        }
    }

    /// <summary>
    /// Unity调用安卓静态方法无返回值
    /// </summary>
    /// <param name="aMethodName"></param>
    /// <param name="content"></param>
    public static void CallStatic(string aMethodName, params object[] content)
    {
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jo.CallStatic(aMethodName, content);
            }
        }
    }

    /// <summary>
    ///  Unity调用安卓方法有返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="aMethodName"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static T Call<T>(string aMethodName, params object[] content)
    {
        T t;
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                t = jo.Call<T>(aMethodName, content);
            }
        }
        return t;
    }

    /// <summary>
    ///  Unity调用安卓静态方法有返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="aMethodName"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static T CallStatic<T>(string aMethodName, params object[] content)
    {
        T t;
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                t = jo.CallStatic<T>(aMethodName, content);
            }
        }
        return t;
    }
}
