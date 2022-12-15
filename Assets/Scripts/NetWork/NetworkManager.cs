using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 网络管理类
/// </summary>
public class NetworkManager
{
   /// <summary>
   /// 超时
   /// </summary>
   private int _timeout = 5; 
   /// <summary>
   /// 请求队列
   /// </summary>
   private Queue<NetworkData> _reqQueue = null;
   /// <summary>
   /// 请求状态
   /// </summary>
   private ReqStateEnum _reqState;
   /// <summary>
   /// Post 表单
   /// </summary>
   private WWWForm _postForm = null;
   /// <summary>
   /// 请求API
   /// </summary>
   private string _reqApi = string.Empty;

   private MonoBehaviour _mono;
   private static NetworkManager _instance;

   private NetworkManager()
   {
      _reqState = ReqStateEnum.IDEL;
      _reqQueue = new Queue<NetworkData>();
   }

   public static NetworkManager Instance
   {
      get
      {
         if (null == _instance) _instance = new NetworkManager();
         return _instance;
      }
   }
   
   /// <summary>
   /// 初始化
   /// </summary>
   /// <param name="mono"></param>
   public void Init(MonoBehaviour mono)
   {
      _mono = mono;
   }

   /// <summary>
   /// 发送请求
   /// </summary>
   /// <param name="data"></param>
   public void SendRequest(NetworkData data)
   {
      if (null == _mono)
      {
         Debug.Log("请调用Init(MonoBehaviour mono)进行初始化");
         return;
      }

      if (_reqState == ReqStateEnum.IDEL)
      {
         _mono.StartCoroutine(ExecuteRequest(data));
      }
      else
      {
         _reqQueue.Enqueue(data);
      }
   }

   /// <summary>
   /// 执行请求
   /// </summary>
   /// <param name="data"></param>
   private IEnumerator ExecuteRequest(NetworkData data)
   {
      _reqState = ReqStateEnum.RUNING;
      _reqApi = data.ReqApi;
      UnityWebRequest request = CreatRequest(data);
      if (null != request)
      {
         request.timeout = _timeout;
         yield return request.SendWebRequest();
         if (request.isDone)
         {
            if (data.IsDispose) data.Dispose();
            DisposeResult(request);
         }
      }
   }

   /// <summary>
   /// 创建请求
   /// </summary>
   /// <param name="data"></param>
   /// <returns></returns>
   private UnityWebRequest CreatRequest(NetworkData data)
   {
      string realUrl = data.Url;
      UnityWebRequest request = null;
      if (data.ReqType == HttpRequestType.GET)
      {
         if (data.ReqData.Count > 0)
         {
            realUrl += "?";  
         }

         int index = 1;
         int dCount = data.ReqData.Count;
         foreach (var reqData in data.ReqData)
         {
            realUrl += $"{reqData.Key}={reqData.Value}";
            if (index < dCount)
            {
               realUrl += "&";
            }
            index++;
         }
         Debug.Log($"==>> send get request: url={realUrl}");
         request = UnityWebRequest.Get(realUrl);
      }
      else if (data.ReqType == HttpRequestType.POST)
      {
         _postForm = new WWWForm();
         foreach (var reqData in data.ReqData)
         {
            _postForm.AddField(reqData.Key,reqData.Value.ToString());
         }
         Debug.Log($"==>> send post request: url={data.Url}  data={_postForm}");
         request = UnityWebRequest.Post(realUrl,_postForm);
      }
   
      if (data.ReqHeader.Count > 0 && null != request)
      {
         foreach (var hData in data.ReqHeader)
         {
            request.SetRequestHeader(hData.Key,hData.Value);
         }
      }

      return request;
   }

   /// <summary>
   /// 处理请求结果
   /// </summary>
   /// <param name="result"></param>
   private void DisposeResult(UnityWebRequest result)
   {
      if (result.result == UnityWebRequest.Result.Success)
      {
         Debug.Log($"===result:{result.downloadHandler.text}");
         _reqState = ReqStateEnum.FINISH;
         //todo: 逻辑处理，是采用回调还是派发消息需要完善逻辑
         //xxx.Instance.DispatchNotice(_reqApi,result.downloadHandler.text);
      }
      else
      {
         Debug.LogError($"Network error:{result.result}");
      }
      NextRequest(); // 下一个请求
   }

   /// <summary>
   /// 下一个请求
   /// </summary>
   private void NextRequest()
   {
      if (_reqState == ReqStateEnum.FINISH)
      {
         if (_reqQueue.Count > 0)
         {
            _mono.StartCoroutine(ExecuteRequest(_reqQueue.Dequeue()));
         }
         else
         {
            _reqState = ReqStateEnum.IDEL;
         }
      }
   }

   /// <summary>
   /// 请求状态枚举
   /// </summary>
   private enum ReqStateEnum
   {
      IDEL,
      RUNING,
      FINISH
   }
}
