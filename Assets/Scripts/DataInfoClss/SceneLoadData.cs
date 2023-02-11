using System.Collections.Generic;
using UnityEngine;
public class SceneLoadData
{
	/// <summary>
	///加载的数据id
	/// </summary>
	public int id;
	/// <summary>
	///属于哪个场景的数据
	/// </summary>
	public string sceneName;
	/// <summary>
	///需要加载的单位名称列表
	/// </summary>
	public List<int> unitIdList;
	/// <summary>
	///单位加载的位置列表
	/// </summary>
	public List<Vector3> positionList;
	/// <summary>
	///需要条件的单位列表
	/// </summary>
	public List<int> conditionUnitList;
	/// <summary>
	///需要的条件列表
	/// </summary>
	public List<string> conditionList;
	/// <summary>
	///需要条件的单位位置列表
	/// </summary>
	public List<Vector3> conditionUnitPositionList;
}
