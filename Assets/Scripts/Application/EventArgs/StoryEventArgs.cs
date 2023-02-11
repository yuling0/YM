using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 剧情事件参数类
/// </summary>
public class StoryEventArgs : EventArgs
{
    public string stringArgs;   //事件参数
    public UnityAction onEventEndCallback;   //事件完成后的回调

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
