using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    /// <summary>
    /// 观察黑板上值的变换的节点
    /// </summary>
    public class BlackboardCondition : ObservingDecorator
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
        public string key;     //要观察的键名
        public ISharedType value;   //条件值
        public E_OperatorType operatorType;

        public BlackboardCondition(string key, ISharedType value, E_OperatorType operatorType, E_StopPolicy stopPolicy): base("BlockboardCondition", stopPolicy)
        {
            this.key = key;
            this.value = value;
            this.operatorType = operatorType;
        }
        public BlackboardCondition(string key, ISharedType value, E_OperatorType operatorType, E_StopPolicy stopPolicy, Node decorator) : base("BlockboardCondition", stopPolicy, decorator)
        {
            this.key = key;
            this.value = value;
            this.operatorType = operatorType;
        }

        public BlackboardCondition() : base("BlockboardCondition")
        {
        }

        protected override bool IsConditionMet()
        {
            Blackboard blackboard = this.Root.Blackboard;
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
                    else if (val is SharedFloat)
                    {
                        return (SharedFloat)val < (SharedFloat)value;
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

        private void OnValueChanged(E_ChangeType changeType, ISharedType value)
        {
            Evaluate();
        }

        public override void Clear()
        {
            base.Clear();
            operatorType = E_OperatorType.IS_EQUAL;
            key = null;
            value = null;
        }
    }
}