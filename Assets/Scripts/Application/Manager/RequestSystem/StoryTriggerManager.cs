using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public class StoryTriggerManager : SingletonBase<StoryTriggerManager>
{
    //TODO:后面根据存档数据读取任务列表数据
    //public void Init()
    //{

    //}
    MissionDataContainer missionDataContainer;
    StoryEventDataContainer storyEventDataContainer;
    ConditionManager conditionManager;
    EventMgr eventMgr;
    private StoryTriggerManager()
    {
        missionDataContainer = BinaryDataManager.Instance.GetContainer<MissionDataContainer>();
        storyEventDataContainer = BinaryDataManager.Instance.GetContainer<StoryEventDataContainer>();
        conditionManager = ConditionManager.Instance;
        eventMgr = EventMgr.Instance;
    }

    /// <summary>
    /// 检查与当前单位对话是否有任务触发
    /// </summary>
    /// <param name="requestName"></param>
    /// <param name="unitName"></param>
    /// <returns></returns>
    public bool CheckUnitHasQuestTrigger(string requestName , string unitName)
    {
        MissionData missionData = missionDataContainer.GetMissionData(requestName);
        if (missionData == null)
        {
            Debug.Log($"未包含任务名为：{requestName}的任务");
            return false;
        }
        //TODO:判断任务状态是否已经触发
        bool res = false;
        string[] conditionStr = missionData.triggerCondition.Split('&');

        //触发条件不对 或 单位名不对
        //if (conditionStr[0] != Consts.RT_DialogBegin || conditionStr[1] != unitName) return false;

        //检查触发该任务的所需条件是否满足
        res = conditionManager.CheckConditionIsTrue(missionData.triggerRequest);

        //任务成功触发
        if (res)
        {
            //TODO:处理触发对话内容、改变任务状态
        }

        return res;
    }

    public bool CheckDialogEndHasQuestComplete(string requestName, string dialogDataName)
    {
        MissionData missionData = missionDataContainer.GetMissionData(requestName);
        if (missionData == null)
        {
            Debug.Log($"未包含任务名为：{requestName}的任务");
            return false;
        }
        //TODO:判断任务状态是否已经触发
        bool res = false;
        string[] conditionStr = missionData.completeCondition.Split('&');

        //触发条件不对 或 对话数据不对
        //if (conditionStr[0] != Consts.RT_DialogBegin || conditionStr[1] != dialogDataName) return false;

        //检查触发该任务的所需条件是否满足
        res = conditionManager.CheckConditionIsTrue(missionData.triggerRequest);

        //任务成功触发
        if (res)
        {
            //TODO:处理触发对话内容、改变任务状态
        }

        return res;
    }

    //这里可以整合在一起（两个方法）
    /// <summary>
    /// 检查场景切换之前是否有事件触发
    /// </summary>
    /// <param name="eventId">事件id</param>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    public bool CheckEventTriggerOnSceneLoadBefore(int eventId , int sceneId)
    {
        var eventData = storyEventDataContainer.GetStoryEventData(eventId);
        if (eventData == null)
        {
            Debug.Log($"未包含事件id为：{eventId}的事件");
            return false;
        }
        //触发条件不对 或 场景名不对
        if (eventData.triggerType != Consts.ET_OnSceneLoadBefore || eventData.triggerCondition != sceneId.ToString()) return false;

        //res = conditionManager.CheckConditionIsTrue(eventData.requestConditionList);
        //判断所需条件是否满足
        foreach (KeyValuePair<string,string> rc in eventData.requestConditions)
        {
            if (!eventMgr.OnEventTrigger<string,bool>(rc.Key,rc.Value))
            {
                return false;
            }
        }

        var eventArgs = StoryEventArgs.Create(eventData.triggerEventArgs, () =>
        {
            //执行事件完成之后的回调函数
            eventMgr.OnEventTrigger(eventData.onEventEndCallbackName, eventData.eventEndArgs);
        });

        eventMgr.OnEventTrigger(eventName: eventData.triggerEventName, eventArgs);

        return true;
    }

    /// <summary>
    /// 检查场景切换之后是否有事件触发
    /// </summary>
    /// <param name="eventId">事件id</param>
    /// <param name="sceneName">场景名</param>
    public bool CheckEventTriggerOnSceneLoadComplete(int eventId , int sceneId)
    {
        var eventData = storyEventDataContainer.GetStoryEventData(eventId);
        if (eventData == null) 
        {
            Debug.Log($"未包含事件id为：{eventId}的事件");
            return false;
        }
        //触发条件不对 或 场景名不对
        if (eventData.triggerType != Consts.ET_OnSceneLoadComplete || eventData.triggerCondition != sceneId.ToString()) return false;

        //判断所需条件是否满足
        foreach (KeyValuePair<string, string> rc in eventData.requestConditions)
        {
            if (!eventMgr.OnEventTrigger<string, bool>(rc.Key, rc.Value))
            {
                return false;
            }
        }

        var eventArgs = StoryEventArgs.Create(eventData.triggerEventArgs, () =>
        {
            //执行事件完成之后的回调函数
            eventMgr.OnEventTrigger(eventData.onEventEndCallbackName, eventData.eventEndArgs);
        });

        eventMgr.OnEventTrigger(eventName: eventData.triggerEventName, eventArgs);

        return true;
    }
}
