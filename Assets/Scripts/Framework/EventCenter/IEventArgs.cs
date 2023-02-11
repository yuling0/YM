using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEventArgs:IReference
{

}
public abstract class EventArgs: IEventArgs 
{
    public object Sender { get; set; }

    public abstract void Clear();
}

/// <summary>
/// 选项的自定义事件参数
/// </summary>
[System.Serializable]
public abstract class SelectionEventArgs : IEventArgs
{
    public abstract void Clear();
}
[System.Serializable]
public class TestEventArgs : SelectionEventArgs
{
    public int i;
    public string str;
    public Sprite icon;

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
