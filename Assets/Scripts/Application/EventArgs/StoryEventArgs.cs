using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// �����¼�������
/// </summary>
public class StoryEventArgs : EventArgs
{
    public string stringArgs;   //�¼�����
    public UnityAction onEventEndCallback;   //�¼���ɺ�Ļص�

    public static StoryEventArgs Create(string stringArgs,UnityAction onEventEndCallback)
    {
        StoryEventArgs args = ReferencePool.Instance.Acquire<StoryEventArgs>();
        args.stringArgs = stringArgs;
        args.onEventEndCallback = onEventEndCallback;
        return args;
    }
    public override void Clear()
    { 
        stringArgs = string.Empty;
        onEventEndCallback = null;
    }


}
