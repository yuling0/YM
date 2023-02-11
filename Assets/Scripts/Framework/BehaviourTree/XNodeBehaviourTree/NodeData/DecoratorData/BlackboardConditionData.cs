using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class BlackboardConditionData : NodeDataBase
{
    public string key;
    public ISharedType data;
    public string operatorType;
    public string stopPolicy;
    public override Decorator CreateDecorator()
    {
        BlackboardCondition blackboardCondition = ReferencePool.Instance.Acquire<BlackboardCondition>();
        blackboardCondition.key = key;
        blackboardCondition.value = data;
        blackboardCondition.operatorType = Extension.Parse<E_OperatorType>(operatorType);
        blackboardCondition.stopPolicy = Extension.Parse<E_StopPolicy>(stopPolicy);
        return blackboardCondition;
    }
}
