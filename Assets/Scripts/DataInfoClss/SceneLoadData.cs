using System.Collections.Generic;
using UnityEngine;
public class SceneLoadData
{
	/// <summary>
	///加载的数据id
	/// </summary>
	public int id;
	/// <summary>
	///需要加载的怪物数据id列表
	/// </summary>
	public List<int> monsterLoadIdList;
	/// <summary>
	///需要加载的npc数据id列表
	/// </summary>
	public List<int> npcLoadIdList;
}
