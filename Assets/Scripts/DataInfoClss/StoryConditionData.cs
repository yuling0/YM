using System.Collections.Generic;
using UnityEngine;
public class StoryConditionData
{
	/// <summary>
	///条件的id
	/// </summary>
	public int id;
	/// <summary>
	///条件名
	/// </summary>
	public string conditionName;
	/// <summary>
	///条件需求
	/// </summary>
	public List<string> conditionRequest;
	/// <summary>
	///条件值
	/// </summary>
	public bool val;
}
