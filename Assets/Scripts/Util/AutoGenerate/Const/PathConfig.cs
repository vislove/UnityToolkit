using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 路径配置
/// </summary>
public class PathConfig
{
    //ResetCore根目录
    public static readonly string ScriptTemplatePath = Application.dataPath + "/Scripts/ScriptTemplate/";

    private static string _projectPath;
    /// <summary>
    /// 工程目录
    /// </summary>
    public static string projectPath
    {
        get
        {
            if (_projectPath == null)
            {
                DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
                _projectPath = directory.Parent.FullName.Replace("\\", "/");
            }
            return _projectPath;
        }
    }
}
