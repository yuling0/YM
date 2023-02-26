using System.Collections.Generic;
using UnityEngine;
public class UnitData
{
	/// <summary>
	///单位id
	/// </summary>
	public int id;
	/// <summary>
	/// 单位名称（键值）
	/// </summary>
	public string unitName;
	/// <summary>
	///资源路径（Resource路径下）
	/// </summary>
	public string resourcePath;
	/// <summary>
	///单位类型（0：玩家、1：敌人、2：NPC 、3 : 其他）
	/// </summary>
	public E_UnitType unitCamp;
	/// <summary>
	///在场景是否是唯一的（不是唯一的，生成unit时，不会使用这里id字段）
	/// </summary>
	public bool isUnique;
}
