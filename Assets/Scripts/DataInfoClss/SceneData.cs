using System.Collections.Generic;
using UnityEngine;
public class SceneData
{
	/// <summary>
	///场景ID
	/// </summary>
	public int sceneId;
	/// <summary>
	///场景名称
	/// </summary>
	public string sceneName;
	/// <summary>
	///当前场景的场景切换单位对应的位置（id ： position）
	/// </summary>
	public Dictionary<int,Vector3> sceneSwitchUnitAndPositionsMap;
	/// <summary>
	///上午场景默认的加载数据的id（对应SceneLoadData表中的id）
	/// </summary>
	public int amSceneLoadDataID;
	/// <summary>
	///场景切换单位与场景的映射
	/// </summary>
	public Dictionary<int,string> sceneSwicthUnitAndSceneMap;
	/// <summary>
	///下午场景默认的加载数据的id（对应SceneLoadData表中的id）
	/// </summary>
	public int pmSceneLoadDataID;
	/// <summary>
	///晚上场景默认的加载数据的id（对应SceneLoadData表中的id）
	/// </summary>
	public int nightSceneLoadDataID;
}
