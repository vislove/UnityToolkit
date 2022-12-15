using System.Collections.Generic;

/// <summary>
/// 网络数据
/// </summary>
public class NetworkData
{
    /// <summary>
    /// 请求数据
    /// </summary>
    private Dictionary<string, object> _reqData = null;
    /// <summary>
    /// 请求头数据
    /// </summary>
    private Dictionary<string, string> _reqHeader = null;

    /// <summary>
    /// 请求类型
    /// </summary>
    private HttpRequestType _reqType = HttpRequestType.POST;

    /// <summary>
    /// 请求源
    /// </summary>
    private object _reqSource;

    /// <summary>
    /// 请求API
    /// </summary>
    private string _reqApi = string.Empty;

    /// <summary>
    /// 自动释放
    /// </summary>
    private bool _isDispose = true;

    public NetworkData()
    {
        _reqData = new Dictionary<string, object>();
        _reqHeader = new Dictionary<string, string>();
    }

    /// <summary>
    /// 请求数据
    /// </summary>
    public Dictionary<string, object> ReqData
    {
        get => _reqData;
    }

    /// <summary>
    /// 请求头
    /// </summary>
    public Dictionary<string, string> ReqHeader
    {
        get => _reqHeader;
    }

    /// <summary>
    /// 请求类型
    /// </summary>
    public HttpRequestType ReqType
    {
        get => _reqType;
        set => _reqType = value;
    }

    /// <summary>
    /// 请求源
    /// </summary>
    public object ReqSource
    {
        get => _reqSource;
        set => _reqSource = value;
    }

    /// <summary>
    /// 请求API
    /// </summary>
    public string ReqApi
    {
        set => _reqApi = value;
        get => _reqApi;
    }

    /// <summary>
    /// 是否释放
    /// </summary>
    public bool IsDispose
    {
        get => _isDispose;
        set => _isDispose = value;
    }

    /// <summary>
    /// url
    /// </summary>
    public string Url
    {
        get => $"{NetworkAPI.BASEURL}{_reqApi}";
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddData(string key, object value)
    {
        _reqData[key] = value;
    }
    
    /// <summary>
    /// 添加请求头
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddHeader(string key, string value)
    {
        _reqHeader[key] = value;
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        _reqData = null;
        _reqHeader = null;
    }
}

/// <summary>
/// Http请求类型
/// </summary>
public enum HttpRequestType
{
    GET,
    POST
}