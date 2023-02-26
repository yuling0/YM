using System.Collections.Generic;
using UnityEngine;
public class PlayerSkillData
{
	/// <summary>
	///技能id
	/// </summary>
	public int id;
	/// <summary>
	///技能名称（用于配置表，代码用不到）
	/// </summary>
	public string skillName;
	/// <summary>
	///伤害百分比
	/// </summary>
	public float damagePercentage;
	/// <summary>
	/// 击退值
	/// </summary>
	public float knockbackValue;
	/// <summary>
	///击飞值
	/// </summary>
	public float knockupVlaue;
	/// <summary>
	///能否防御
	/// </summary>
	public bool isDefendable;
}
