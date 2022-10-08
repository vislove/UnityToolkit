using UnityEngine;
using System.Collections;

public enum Platform
{
    OSXEditor = 0,
    OSXPlayer,
    WindowsPlayer,
    WindowsEditor,
    IPhonePlayer,
    Android,
    Unkown,
}
/// <summary>
/// App 平台及路径
/// </summary>
public static class AppPlatform 
{
    /// <summary>
    /// 平台初始化
    /// </summary>
    public static void Initialize()
    {
        AppPlatform.PlatformCurrent = RuntimePlatform_To_AppPlaform(Application.platform);
    }

    public static string[] PlatformPathPrefixs = 
    {
        "file:///",         //OSXEditor
        "file:///",         //OSXPlayer
        "file:///",         //WindowsPlayer
        "file:///",         //WindowsEditor
        "file://",          //IphonePlayer
        "file:///",         //Android
    };

    public static string[] PlatformNames = 
    {
        "Windows",
        "OSX",
        "Windows",
        "Windows",
        "IOS",
        "Android"
    };

    public static bool[] PlatformIsEditor = 
    {
        true,
        false,
        false,
        true,
        false,
        false
    };

    public static Platform PlatformCurrent { set; get; }

    /// <summary>
    /// 运行时资源路径，解包后
    /// </summary>
    public static string RuntimeAssetsPath
    {
        get
        {
            ///< 移动平台走沙盒
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/";
            }
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 获取工程目录 Assets上级路径
    /// </summary>
    public static string GetProjectPath
    {
        get
        {
            return Application.dataPath + "/../";
        }
    }

    public static string GetPackagePath()
    {
        return Application.streamingAssetsPath + "/" + GetPackageName() + "/";
    }

    public static string GetPackageName()
    {
        int index = (int)AppPlatform.PlatformCurrent;     
        return AppPlatform.PlatformNames[index];
    }
    
    public static string GetRuntimePackagePath()
    {
        return RuntimeAssetsPath + GetPackageName() + "/";
    }
    
    public static string GetStreamingAssetsPath
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    public static string GetUIPanelJsonFilePath
    {
        get
        {
            return Application.streamingAssetsPath + "/Json";
        }
    }

    public static string GetFormPath
    {
        get
        {
            return Application.persistentDataPath;
        }
    }

    /// <summary>
    /// 运行平台
    /// </summary>
    /// <returns></returns>
    public static string GetRuntimeModelPackgePath() 
    {
        return RuntimeAssetsPath + GetModelPackageName() + "/";
    }

    /// <summary>
    /// 运行平台
    /// </summary>
    /// <returns></returns>
    public static string GetModelPackageName() 
    {
        int index = (int)AppPlatform.PlatformCurrent;
        return AppPlatform.PlatformNames[index];
    }

    public static string GetRuntimeAssetBundleUrl()
    {
        int index = (int)AppPlatform.PlatformCurrent;
        return AppPlatform.PlatformPathPrefixs[index] + RuntimeAssetsPath + GetPackageName() + "/" + GetAssetBundleDirName() + "/";
    }

    public static string GetServerAssetBundleUrl()
    {
        int index = (int)AppPlatform.PlatformCurrent;
        return AppPlatform.PlatformNames[index].ToLower() + "/";
    }

    public static string PlatformCurrentName
    {
        get { return PlatformNames[(int)AppPlatform.PlatformCurrent]; }
    }

    public static string GetRuntimeLevelNorm
    {
        get
        {
            int index = (int)AppPlatform.PlatformCurrent;
            return AppPlatform.PlatformPathPrefixs[index] + RuntimeAssetsPath;
        }
    }

    public static string GetRuntimeChecklist
    {
        get
        {
            int index = (int)AppPlatform.PlatformCurrent;
            return AppPlatform.PlatformPathPrefixs[index] + GetStreamingAssetsPath;
        }
    }

    private static Platform RuntimePlatform_To_AppPlaform(RuntimePlatform runtimePlatform)
    {
        switch (runtimePlatform)
        {
            case UnityEngine.RuntimePlatform.Android: return Platform.Android;
            case UnityEngine.RuntimePlatform.IPhonePlayer: return Platform.IPhonePlayer;
            case UnityEngine.RuntimePlatform.OSXEditor: return Platform.OSXEditor;
            case UnityEngine.RuntimePlatform.OSXPlayer: return Platform.OSXPlayer;
            case UnityEngine.RuntimePlatform.WindowsEditor: return Platform.WindowsEditor;
            case UnityEngine.RuntimePlatform.WindowsPlayer: return Platform.WindowsPlayer;
            default: return Platform.Unkown;
        }
    }

    public static string GetAssetBundleDirName()
    {
        return "AssetBundles";
    }

    public static string GetAssetBundleDirModelName() 
    {
        return "ModelBundles"; //Windows
    }

}