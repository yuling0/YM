using System.Collections.Generic;
using UnityEngine;
public class SceneSwitchUnitData
{
	/// <summary>
	///场景单位的ID
	/// </summary>
	public int unitID;
	/// <summary>
	///该场景单位切换的目标场景名称
	/// </summary>
	public string switchSceneName;
	/// <summary>
	///切换到目标场景后，玩家的位置
	/// </summary>
	public Vector3 position;
}
