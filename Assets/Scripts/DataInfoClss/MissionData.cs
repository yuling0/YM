using System.Collections.Generic;
using UnityEngine;
public class MissionData
{
	/// <summary>
	///主线ID
	/// </summary>
	public int id;
	/// <summary>
	///任务名称
	/// </summary>
	public string missionName;
	/// <summary>
	///任务状态（0：未接取1：进行中2：已完成）
	/// </summary>
	public E_RequestState state;
	/// <summary>
	///触发时需要的对话数据名称
	/// </summary>
	public string dialogDataName;
	/// <summary>
	///触发条件
	/// </summary>
	public string triggerCondition;
	/// <summary>
	///触发任务需要的条件（触发任务的需求）
	/// </summary>
	public List<string> triggerRequest;
	/// <summary>
	///完成任务的需求
	/// </summary>
	public List<string> completeRequest;
	/// <summary>
	///完成条件
	/// </summary>
	public string completeCondition;
}
