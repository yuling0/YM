using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : SingletonBase<StorySystem>
{
    //TODO:当前进行中的章节（后面需要读本地数据）
    ChapterData curChapter;
    StoryTriggerManager storyTriggerManager;
    E_TimeBucket timeBucket;

    public E_TimeBucket TimeBucket => timeBucket;

    private void SetTimeBucket(string timeBuckectStr)
    {
        timeBucket = Extension.Parse<E_TimeBucket>(timeBuckectStr);
    }
    //TODO:后续可能会写一个总的游戏管理器，管理这些一个个单个管理器（通过依赖注入方式传引用）
    private StorySystem()
    {
        storyTriggerManager = StoryTriggerManager.Instance;
        //测试
        curChapter = BinaryDataManager.Instance.GetContainer<ChapterDataContainer>().GetChapterData("Chapter1");
    }

    //public void Init(ChapterData chapterData)
    //{
    //    curChapter = chapterData;

    //}
    /// <summary>
    /// 当和某个NPC对话时，判断任务是否触发
    /// </summary>
    /// <param name="unitName">NPC的名字</param>
    public bool OnDialogBegin(int unitId)
    {
        bool res = false;
        foreach (var missionName in curChapter.missionList)
        {
            //res |= storyTriggerManager.CheckUnitHasQuestTrigger(missionName, unitName);
            //最多一个任务触发（设定）
            if (res)
            {
                break;
            }
        }
        return res;
    }

    /// <summary>
    /// 当对话结束时，判断任务是否完成
    /// </summary>
    private void OnDialogEnd(string dialogDataName)
    {
        
    }

    /// <summary>
    /// 检查当前场景切换时，是否有事件需要触发
    /// </summary>
    /// <param name="sceneName">需要切换的场景名</param>
    /// <returns></returns>
    public bool CheckEventTriggerOnSceneLoadBefore(int sceneId)
    {
        bool res = false;
        foreach (var id in curChapter.eventIdList)
        {
            res |= storyTriggerManager.CheckEventTriggerOnSceneLoadBefore(id, sceneId);
            if (res)
            {
                break;
            }
        }
        return res;
    }
    /// <summary>
    /// 检查当前场景切换后，是否有事件需要触发
    /// </summary>
    /// <param name="sceneName">需要切换的场景名</param>
    /// <returns></returns>
    public bool CheckEventTriggerOnSceneLoadComplete(int sceneId)
    {
        bool res = false;
        foreach (var id in curChapter.eventIdList)
        {
            res |= storyTriggerManager.CheckEventTriggerOnSceneLoadComplete(id, sceneId);
            if (res)
            {
                break;
            }
        }
        return res;
    }

    public bool CheckEventTriggerOnCutscenePlayed(string cutsceneName)
    {
        bool res = false;
        foreach (var id in curChapter.eventIdList)
        {
            res |= storyTriggerManager.CheckEventTriggerOnCutscenePlayed(id, cutsceneName);
            if (res)
            {
                break;
            }
        }
        return res;
    }

    public int GetSceneLoadDataID(int sceneId)
    {
        int res = -1;
        switch (timeBucket)
        {
            case E_TimeBucket.AM:
                if (curChapter.amSceneLoadDataMap.ContainsKey(sceneId))
                {
                    res = curChapter.amSceneLoadDataMap[sceneId];
                }
                break;
            case E_TimeBucket.PM:
                if (curChapter.pmSceneLoadDataMap.ContainsKey(sceneId))
                {
                    res = curChapter.amSceneLoadDataMap[sceneId];
                }
                break;
            case E_TimeBucket.Night:
                if (curChapter.nightSceneLoadDataMap.ContainsKey(sceneId))
                {
                    res = curChapter.amSceneLoadDataMap[sceneId];
                }
                break;
        }
        return res;
    }
}
