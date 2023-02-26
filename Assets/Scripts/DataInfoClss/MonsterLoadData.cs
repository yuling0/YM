using System.Collections.Generic;
using UnityEngine;
public class MonsterLoadData
{
	/// <summary>
	///数据id（主要为了唯一性）
	/// </summary>
	public int id;
	/// <summary>
	///需要加载的怪物id（对应UnitData表中的id）
	/// </summary>
	public int unitId;
	/// <summary>
	///怪物生成位置（出生点）
	/// </summary>
	public Vector3 birthPoint;
	/// <summary>
	///各个巡逻点坐标
	/// </summary>
	public List<Vector3> patrolPoints;
}
