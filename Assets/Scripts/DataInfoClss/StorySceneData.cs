using System.Collections.Generic;
using UnityEngine;
public class StorySceneData
{
	/// <summary>
	///剧情场景数据id
	/// </summary>
	public int id;
	/// <summary>
	///需要加载的场景名称
	/// </summary>
	public string sceneName;
	/// <summary>
	///玩家的位置
	/// </summary>
	public Vector3 playerPosition;
	/// <summary>
	///玩家是否朝右
	/// </summary>
	public bool playerIsFacingRight;
	/// <summary>
	///需要加载的npc数据id列表
	/// </summary>
	public List<int> npcLoadIdList;
	/// <summary>
	///需要加载的怪物数据id列表
	/// </summary>
	public List<int> monsterLoadIdList;
	/// <summary>
	///是否需要设置摄像机位置（一般播放过场动画时，需要设置摄像机初始位置）
	/// </summary>
	public bool isNeedSetCameraPosition;
	/// <summary>
	///摄像机位置
	/// </summary>
	public Vector3 cameraPosition;
}
