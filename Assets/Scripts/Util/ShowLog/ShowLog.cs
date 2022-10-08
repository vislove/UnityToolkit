using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 在屏幕上输出日志
/// 使用: 在框架层实例出 ShowLog.prefab 到UI需要的层级；Log按钮可拖拽；Clear按钮可清除日志；
/// 联系Email: soooooor@163.com
/// </summary>
public class ShowLog : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
	#region 组件
	/// <summary>
	/// 显示日志的最大数量
	/// </summary>
	private int maxLogNumber = 20000;
	/// <summary>
	/// 显示日志按钮
	/// </summary>
	private Button showLogBtn;
	/// <summary>
	/// 内容显示Obj
	/// </summary>
	private Transform contentObject;
	/// <summary>
	/// 清除日志按钮
	/// </summary>
	private Button clearLogBtn;
	/// <summary>
	/// 日志内容Text
	/// </summary>
	private Text logContentText;
	/// <summary>
	/// 是否显示Log
	/// </summary>
	/// <returns></returns>
	private bool isShowLog = false;
	/// <summary>
	/// 计数器
	/// </summary>
	private int counter = 0;
	/// <summary>
	/// 日志颜色
	/// </summary>
	private readonly Dictionary<LogType, string> LogColor = new Dictionary<LogType, string>()
	{
		{
			LogType.Assert,
			"#1874CD"
		},
		{
			LogType.Error,
			"#FF0000"
		},
		{
			LogType.Exception,
			"#FF00FF"
		},
		{
			LogType.Log,
			"#00FF00"
		},
		{
			LogType.Warning,
			"#FFFF00"
		}
	};
	#endregion
	
	void Awake()
	{
		showLogBtn = transform.Find("ShowLogButton").GetComponent<Button>();
		contentObject = transform.Find("ConentObject");
		clearLogBtn = contentObject.Find("ClearLogButton").GetComponent<Button>();
		logContentText = contentObject.Find("Viewport/Content/LogContentText").GetComponent<Text>();
	}

	// Use this for initialization
	void Start () 
	{
		showLogBtn.onClick.AddListener(OnShowBtnClick);
		clearLogBtn.onClick.AddListener(OnClearLogBtnClick);
		Application.RegisterLogCallback(HandleLog);	
	}

	#region 点击事件
	/// <summary>
	/// 点击显示日志按钮
	/// </summary>
	private void OnShowBtnClick()
	{
		if(contentObject == null) return;
		if (isShowLog)
		{
			contentObject.gameObject.SetActive(false);
			isShowLog = false;
		}
		else
		{
			contentObject.gameObject.SetActive(true);
			isShowLog = true;
		}
	}
	/// <summary>
	/// 点击清除日志按钮
	/// </summary>
	private void OnClearLogBtnClick()
	{
		logContentText.text = string.Empty;
		counter = 0;
		logContentText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
	}
	
	#endregion
	
	#region 转化需要输出在屏幕上的日志
	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (counter > maxLogNumber)
		{
			OnClearLogBtnClick();
		}
		string logContent = GetLogColorText(type,logString);
		logContentText.text += logContent;
		counter += logContent.Length;
	}
	
	/// <summary>
	/// 获取日志颜色
	/// </summary>
	private string GetLogColorText(LogType type,string logString)
	{
		string str = "\n===>  ";
		str += String.Format("<color={0}>{1}</color>",LogColor[type],logString);
		return str;
	}
	#endregion
	
	# region 按钮拖拽相关
	/// <summary>
	/// 开始拖拽
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		SetDraggedPosition(eventData);
	}

	/// <summary>
	/// 拖拽
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		SetDraggedPosition(eventData);
	}

	/// <summary>
	/// 结束拖拽
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		SetDraggedPosition(eventData);
	}

	/// <summary>
	/// 设置拖拽点
	/// </summary>
	/// <param name="eventData">Event data.</param>
	private void SetDraggedPosition(PointerEventData eventData)
	{
		var rt = showLogBtn.gameObject.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
		}
	}
	#endregion
}
