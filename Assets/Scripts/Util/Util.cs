using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;
using ResetCore.Util;

namespace ResetCore.Util
{
    public class Util : MonoBehaviour
    {
        /// <summary>
        /// 取可读的容量大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String GetReadableByteSize(double size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }

        /// <summary>
        /// 取当前时间
        /// </summary>
        static DateTime ms_date_1970 = new DateTime(1970, 1, 1);
        public static uint GetNowSeconds()
        {
            DateTime d1 = DateTime.Now;
            uint time_time = (uint)d1.Subtract(ms_date_1970).TotalSeconds;
            return time_time;
        }

        public static long GetNowMilliseconds()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        public static string FormatTime(int time)
        {
            int second = time % 60;
            int minute = (time / 60) % 60;
            int hour = time / (60 * 60);

            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }

        /// <summary>
        /// 0-60分钟内，显示xx分钟之前；其中0-2分钟显示1分钟之前，2-3分钟显示2分钟之前，59-60分钟显示59分钟之前。以此类推。
        /// 1-24小时内，显示xx小时之前；
        /// 1-2天内，显示1天之前；
        /// 2天以前，显示具体日期。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ShowTime(int serlverTimestamp, int time)
        {
            int deta = serlverTimestamp - time;
            int minute = (deta / 60) % 60 + 1;
            int hour = deta / (60 * 60);
            int day = deta / (60 * 60 * 24);
            string result = "";
            if (day == 0)
                result = hour == 0 ? string.Format("{0}分钟前", minute) : string.Format("{0}小时前", hour);
            else if (day <= 2)
                result = string.Format("{0}天前", day);
            else
                result = GetTime(time.ToString()).ToString("yyyy-MM-dd HH:mm:ss");

            return result;
        }

        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(ms_date_1970);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        /// <summary>
        /// new 一个新对象
        /// </summary>
        public static GameObject NewObject(string name, GameObject rParent)
        {
            var rGo = new GameObject(name);
            rGo.transform.SetParent(rParent.transform);
            rGo.transform.localPosition = rParent.transform.localPosition;
            rGo.transform.localScale = rParent.transform.localScale;
            rGo.transform.localRotation = rParent.transform.localRotation;
            return rGo;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string MD5File(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        public static string MD5File(byte[] file)
        {
            try
            {
                //FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                //fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string MD5String(string str)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string ret = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)), 4, 8);
            return ret.Replace("-", "");
        }

        /// <summary>
        /// 修正RectTransform
        /// </summary>
        public static void FixInstantiated(Component source, Component instance)
        {
            FixInstantiated(source.gameObject, instance.gameObject);
        }

        /// <summary>
        /// 修正RectTransform
        /// </summary>
        public static void FixInstantiated(GameObject source, GameObject instance)
        {
            var defaultRectTransform = source.GetComponent<RectTransform>();
            var rectTransform = instance.GetComponent<RectTransform>();

            rectTransform.localPosition = defaultRectTransform.localPosition;
            rectTransform.localRotation = defaultRectTransform.localRotation;
            rectTransform.localScale = defaultRectTransform.localScale;
            rectTransform.anchoredPosition = defaultRectTransform.anchoredPosition;
        }

        /// <summary>
        /// 向上搜索Canvas
        /// </summary>
        public static Transform FindCanvas(Transform currentObject)
        {
            var canvas = currentObject.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                return null;
            }
            return canvas.transform;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = FindChild(go, subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        public static Transform FindChild(GameObject go, string name)
        {
            Transform child = go.transform.Find(name);
            if (child != null)
                return child;
            foreach (Transform tran in go.transform)
            {
                child = FindChild(tran.gameObject, name);
                if (child != null)
                    return child;
            }

            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null) DestroyImmediate(ts[i]);
                }
                return go.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component
        {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode)
        {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        private static GameObject Child(Transform go, string subnode)
        {
            Transform tran = go.Find(subnode);
            if (tran == null) { throw new Exception("查找的物体为空！"); }
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode)
        {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode)
        {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        /// <summary>
        /// 得到Path文件夹下所有文件 并放入allFilePath List中
        /// </summary>
        public static void RecursiveDir(string path, ref List<string> allFilePath, bool isFirstRun = true)
        {
            if (isFirstRun && allFilePath.Count > 0)
            {
                allFilePath.TrimExcess();
                allFilePath.Clear();
            }

            string[] names = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string filename in names)
            {
                string ext = Path.GetExtension(filename);
                if (ext.Equals(".meta")) continue;

                allFilePath.Add(filename.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                RecursiveDir(dir, ref allFilePath, false);
            }

        }

        /// <summary>
        /// 获取网络状态
        /// </summary>
        public static bool GetNetworkState
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 获取网络模式 true wifi false 移动蜂窝
        /// </summary>
        public static bool GetNetworkModel
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }
    }
}
