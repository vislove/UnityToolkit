using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkExample : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Instance.Init(this);
    }

    private void Login()
    {
        NetworkData _networkData = new NetworkData();
        _networkData.ReqApi = NetworkAPI.LOGIN;
        _networkData.ReqType = HttpRequestType.POST;
        _networkData.AddData("userName","xxx");
        _networkData.AddData("passwd","xxx");
        NetworkManager.Instance.SendRequest(_networkData);
    }

    private void GetUserLevel()
    {
        NetworkData _networkData = new NetworkData();
        _networkData.ReqApi = NetworkAPI.USER_LEVEL;
        _networkData.ReqType = HttpRequestType.GET;
        _networkData.AddHeader("x-auth","xxxxxx");
        _networkData.AddData("uid",100);
        NetworkManager.Instance.SendRequest(_networkData);
    }
}
