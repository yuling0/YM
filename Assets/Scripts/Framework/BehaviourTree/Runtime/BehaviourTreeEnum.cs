using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public enum State    //当前节点的状态
    {
        INACTIVE,        //未运行
        ACTIVE,          //正在运行
        STOP_REQUESTED   //代表节点正在停止的状态
    }
    public enum E_OperatorType
    {
        IS_SET,
        IS_NOT_SET,
        IS_EQUAL,
        IS_NO_EQUAL,
        IS_GREATER_OR_EQUAL,
        IS_GREATER,
        IS_SMALLER_OR_EQUAL,
        IS_SMALLER,
    }

    public enum E_StopPolicy
    {
        /// <summary>
        /// 默认策略：当子节点不满足条件时停止观察条件
        /// </summary>
        NONE,
        /// <summary>
        /// 终止父节点：即使在不满足条件下也会观察条件是否满足，当此节点未激活并且满足条件时，将会终止父节点的后续执行
        /// </summary>
        AbortParent,
        /// <summary>
        /// 立刻重启：当此节点未激活并且满足条件时，会停止父节点当前正在执行的节点，并立刻启动此节点
        /// </summary>
        IMMEDIATE_RESTART
    }
    public enum E_Policy
    {
        One,
        ALL
    }
    public enum E_Result
    {
        PROCESSING,     //当前多帧Action正在执行
        SUCESS,         //执行成功
        FAILED          //执行失败
    }
}