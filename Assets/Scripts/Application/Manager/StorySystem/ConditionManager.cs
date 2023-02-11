using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剧情条件管理器（存储与剧情触发相关的条件）
/// 判断事件（任务）条件是否满足
/// </summary>
public class ConditionManager : SingletonBase<ConditionManager>
{
    StoryConditionDataContainer dataContainer;
    Dictionary<string, IConditionCheckStrategy> conditionStrategyDic;
    private ConditionManager()
    {
        dataContainer = BinaryDataManager.Instance.GetContainer<StoryConditionDataContainer>();
        conditionStrategyDic = new Dictionary<string, IConditionCheckStrategy>
        {
            { "type=30001", new QuestRelevanceStrategy() },
            { "type=10003", new TestStrategy() }
        };
    }

    public bool GetCondition(string conditionName)
    {
        var conditionData = dataContainer.GetStoryConditionData(conditionName);
        bool res = conditionData.val;

        if (!res)
        {
            res = CheckConditionIsTrue(conditionData.conditionRequest);
        }

        return res;
    }

    public bool CheckConditionIsTrue(string conditionStr)
    {
        if (conditionStr == "null") return true;
        string[] condition = conditionStr.Split('&');

        if (!conditionStrategyDic.ContainsKey(condition[0]))
        {
            Debug.Log("未找到当前任务的策略：{conditionStr[0]}");
            return false;
        }

        return conditionStrategyDic[condition[0]].CheckQuestCondition(condition[1]);
    }
    public bool CheckConditionIsTrue(List<string> conditionList)
    {
        bool res = true;
        foreach (var str in conditionList)
        {
            if (str == "null") return true;

            string[] condition = str.Split('&');

            if (!conditionStrategyDic.ContainsKey(condition[0]))
            {
                Debug.Log("未找到当前任务的策略：{conditionStr[0]}");
                return false;
            }

            res &= conditionStrategyDic[condition[0]].CheckQuestCondition(condition[1]);

            if (!res)
            {
                return res;
            }
        }
        return res;
    }
}
