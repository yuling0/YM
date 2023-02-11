using System.Collections.Generic;
using UnityEngine;
public class StoryEventData
{
	/// <summary>
	///剧情事件ID
	/// </summary>
	public int eventId;
	/// <summary>
	///剧情事件名称
	/// </summary>
	public string eventName;
	/// <summary>
	///事件触发的类型
	/// </summary>
	public string triggerType;
	/// <summary>
	/// 剧情事件触发条件（这个条件是瞬时的，如切换到某个场景的回调）
	/// </summary>
	public string triggerCondition;
	/// <summary>
	///触发这个剧情事件需要具备的条件（这个条件是长时间存在的）
	/// </summary>
	public Dictionary<string,string> requestConditions;
	/// <summary>
	///剧情事件触发后响应的函数名（一般为触发对话）
	/// </summary>
	public string triggerEventName;
	/// <summary>
	///事件参数（没有参数填null）
	/// </summary>
	public string triggerEventArgs;
	/// <summary>
	///该剧情事件是否只能一次触发（true：只能触发一次,false:触发多次）
	/// </summary>
	public bool isOnceTrigger;
	/// <summary>
	///剧情事件完成后要执行的函数回调（没有需要执行的回调也填null）
	/// </summary>
	public string onEventEndCallbackName;
	/// <summary>
	///剧情事件完成后回调函数的参数（没有参数填null）
	/// </summary>
	public string eventEndArgs;
}
