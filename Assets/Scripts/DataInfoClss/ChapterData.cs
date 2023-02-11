using System.Collections.Generic;
using UnityEngine;
public class ChapterData
{
	/// <summary>
	///章节名称
	/// </summary>
	public string chapterName;
	/// <summary>
	///当前章节的主线任务列表
	/// </summary>
	public List<string> missionList;
	/// <summary>
	///当前章节的事件ID列表
	/// </summary>
	public List<int> eventIdList;
	/// <summary>
	///当前章节禁用的场景名称（不能切换的场景名称 与 切换禁用场景时触发的对话数据名称的字典
	/// </summary>
	public Dictionary<string,string> disableSceneNameDic;
	/// <summary>
	///当前章节需要条件解锁的场景名称与条件的字典
	/// </summary>
	public Dictionary<string,string> conditionSceneNameDic;
	/// <summary>
	///上午场景id与需要加载的场景数据id的映射（剧情没有特殊要求，就会加载默认场景数据）
	/// </summary>
	public Dictionary<int,int> amSceneLoadDataMap;
	/// <summary>
	///下午场景id与需要加载的场景数据id的映射（剧情没有特殊要求，就会加载默认场景数据）
	/// </summary>
	public Dictionary<int,int> pmSceneLoadDataMap;
	/// <summary>
	///晚上场景id与需要加载的场景数据id的映射（剧情没有特殊要求，就会加载默认场景数据）
	/// </summary>
	public Dictionary<int,int> nightSceneLoadDataMap;
}
