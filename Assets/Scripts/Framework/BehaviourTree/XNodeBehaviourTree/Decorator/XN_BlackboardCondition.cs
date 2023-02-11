using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;
using static YMFramework.BehaviorTree.BlackboardCondition;

namespace YMFramework.BehaviorTreeEditor
{
    /// <summary>
    /// 观察黑板上值的变换的节点
    /// </summary>
    public class XN_BlackboardCondition : XN_ObservingDecorator
    {
        //public enum E_OperatorType
        //{
        //    IS_SET,
        //    IS_NOT_SET,
        //    IS_EQUAL,
        //    IS_NO_EQUAL,
        //    IS_GREATER_OR_EQUAL,
        //    IS_GREATER,
        //    IS_SMALLER_OR_EQUAL,
        //    IS_SMALLER,
        //}

        //public enum E_ValueType
        //{
        //    Shared_Int,
        //    Shared_Float,
        //    Shared_String,
        //    Shared_Vector2,
        //    Shared_Vector3,
        //    Shared_Object
        //}
        [LabelText("观察的键名")]
        public string key;     //要观察的键名

        //[LabelText("条件值的类型")]
        //public E_ValueType type;

        [LabelText("条件值")]
        public ISharedType value;   //条件值

        [EnumPaging, LabelText("操作类型")]
        public E_OperatorType operatorType;
        //public XN_BlackboardCondition(string key, object value, E_OperatorType operatorType, E_StopPolicy stopPolicy, XN_NodeBase decorator) : base("BlockboardCondition", stopPolicy, decorator)
        //{
        //    this.key = key;
        //    this.value = value;
        //    this.operatorType = operatorType;
        //}

        protected override bool IsConditionMet()
        {
            XN_Blackboard blackboard = this.Root.Blackboard;
            if (!blackboard.IsSet(key))
            {
                if (operatorType == E_OperatorType.IS_NOT_SET)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (operatorType == E_OperatorType.IS_NOT_SET)
                {
                    return false;
                }
                else if (operatorType == E_OperatorType.IS_SET)
                {
                    return true;
                }

                ISharedType val = blackboard[key];

                if (operatorType == E_OperatorType.IS_EQUAL)
                {
                    return val.Equals(value);
                }
                else if (operatorType == E_OperatorType.IS_NO_EQUAL)
                {
                    return !val.Equals(value);
                }
                else if (operatorType == E_OperatorType.IS_GREATER_OR_EQUAL)
                {
                    if (val is SharedFloat)
                    {
                        return (SharedFloat)val >= (SharedFloat)value;
                    }
                    else if (val is SharedInt)
                    {
                        return (SharedInt)val >= (SharedInt)value;
                    }
                    else
                    {
                        Debug.LogError("该类型无法比较大小" + val.GetType());
                    }
                }
                else if (operatorType == E_OperatorType.IS_GREATER)
                {
                    if (val is SharedFloat)
                    {
                        return (SharedFloat)val > (SharedFloat)value;
                    }
                    else if (val is SharedInt)
                    {
                        return (SharedInt)val > (SharedInt)value;
                    }
                    else
                    {
                        Debug.LogError("该类型无法比较大小" + val.GetType());
                    }
                }
                else if (operatorType == E_OperatorType.IS_SMALLER_OR_EQUAL)
                {
                    if (val is SharedFloat)
                    {
                        return (SharedFloat)val <= (SharedFloat)value;
                    }
                    else if (val is SharedInt)
                    {
                        return (SharedInt)val <= (SharedInt)value;
                    }
                    else
                    {
                        Debug.LogError("该类型无法比较大小" + val.GetType());
                    }
                }
                else if (operatorType == E_OperatorType.IS_SMALLER)
                {
                    if (val is SharedFloat)
                    {
                        return (SharedFloat)val < (SharedFloat)value;
                    }
                    else if (val is SharedInt)
                    {
                        return (SharedInt)val < (SharedInt)value;
                    }
                    else
                    {
                        Debug.LogError("该类型无法比较大小" + val.GetType());
                    }
                }
                return false;
            }
        }

        protected override void StartObserving()
        {
            this.Root.Blackboard.AddObserver(key, OnValueChanged);
        }

        protected override void StopObserving()
        {
            this.Root.Blackboard.RemoveObserver(key, OnValueChanged);
        }

        private void OnValueChanged(E_ChangeType changeType, object value)
        {
            Evaluate();
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new BlackboardConditionData()
            { 
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator,
                key = key,
                data = value,
                operatorType = operatorType.ToString(), 
                stopPolicy = stopPolicy.ToString()
            };
        }

        public override BehaviorTree.Node CreateNode()
        {
            BlackboardCondition blackboardCondition = ReferencePool.Instance.Acquire<BlackboardCondition>();
            blackboardCondition.key = key;
            blackboardCondition.value = value;
            blackboardCondition.operatorType = operatorType;
            blackboardCondition.stopPolicy = stopPolicy;
            return blackboardCondition;
        }
    }
}