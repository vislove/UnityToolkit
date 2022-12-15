﻿/// <summary>
/// Network state code
/// </summary>
public class NetStateCode
{
   /// <summary>
   /// 状态码解析失败
   /// </summary>
   public const int CODE_9999 = -100;
   /// <summary>
   /// 主动断开服务器
   /// </summary>
   public const int CODE_9998 = -101;
   /// <summary>
   /// Photon主动断开链接
   /// </summary>
   public const int CODE_9997 = -102;
   /// <summary>
   /// 请求成功
   /// </summary>
   public const int CODE_0000 = 200;
   /// <summary>
   /// 请求失败
   /// </summary>
   public const int CODE_0400 = 400;

   public const int CODE_0401 = 401;
   public const int CODE_0500 = 500;
   public const int CODE_0405 = 405;
}
