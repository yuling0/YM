using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public enum State    //��ǰ�ڵ��״̬
    {
        INACTIVE,        //δ����
        ACTIVE,          //��������
        STOP_REQUESTED   //����ڵ�����ֹͣ��״̬
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
        /// Ĭ�ϲ��ԣ����ӽڵ㲻��������ʱֹͣ�۲�����
        /// </summary>
        NONE,
        /// <summary>
        /// ��ֹ���ڵ㣺��ʹ�ڲ�����������Ҳ��۲������Ƿ����㣬���˽ڵ�δ�������������ʱ��������ֹ���ڵ�ĺ���ִ��
        /// </summary>
        AbortParent,
        /// <summary>
        /// �������������˽ڵ�δ�������������ʱ����ֹͣ���ڵ㵱ǰ����ִ�еĽڵ㣬�����������˽ڵ�
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
        PROCESSING,     //��ǰ��֡Action����ִ��
        SUCESS,         //ִ�гɹ�
        FAILED          //ִ��ʧ��
    }
}