using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网络请求接口
/// </summary>
public class NetworkAPI
{
    /// <summary>
    /// URL
    /// </summary>
    public const string BASEURL = "http://xxxx/api/";

    #region Example
    // Login
    public const string LOGIN = "client/login";
    /// <summary>
    /// User level
    /// </summary>
    public const string USER_LEVEL = "client/level";
    #endregion
}

/// <summary>
/// 网络事件常量
/// </summary>
public class NetworkConstant
{   
    // 请求激活列表
    //public const string REQ_ACTIVATE_LIST = "req_activate_list";
}