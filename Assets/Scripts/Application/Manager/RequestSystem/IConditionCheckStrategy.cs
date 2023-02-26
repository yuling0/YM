using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任务完成或触发的需求检测策略
/// </summary>
public interface IConditionCheckStrategy
{
    public bool CheckQuestCondition(string conditionStr);
}
/// <summary>
/// 任务需求策略（需要前置任务需求的策略）
/// </summary>
public class QuestRelevanceStrategy : IConditionCheckStrategy
{
    //需要某个任务到达指定状态
    public bool CheckQuestCondition(string conditionStr)
    {
        throw new System.NotImplementedException();
    }
}

public class TestStrategy : IConditionCheckStrategy
{
    //需要某个任务到达指定状态
    public bool CheckQuestCondition(string conditionStr)
    {
        return false;
    }
}
