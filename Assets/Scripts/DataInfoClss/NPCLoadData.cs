using System.Collections.Generic;
using UnityEngine;
public class NPCLoadData
{
	/// <summary>
	///数据id（主要为了唯一性）
	/// </summary>
	public int id;
	/// <summary>
	///需要加载的npc的id（对应UnitData表中的id）
	/// </summary>
	public int unitId;
	/// <summary>
	///npc名称（方便自己看的，游戏里没使用这数据）
	/// </summary>
	public string npcName;
	/// <summary>
	///是否朝向右边
	/// </summary>
	public bool isFacingRight;
	/// <summary>
	///NPC的生成位置
	/// </summary>
	public Vector3 npcPosition;
}
