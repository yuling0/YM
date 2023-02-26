using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueArgs : EventArgs
{
    private string dialogueName;
    private UnityAction onCompleted;

    public string DialogueName => dialogueName; 
    public UnityAction OnCompleted => onCompleted;

    public static DialogueArgs Create(string dialogueName,UnityAction OnCompleted)
    {
        DialogueArgs dialogueArgs = ReferencePool.Instance.Acquire<DialogueArgs>();
        dialogueArgs.dialogueName= dialogueName;
        dialogueArgs.onCompleted= OnCompleted;
        return dialogueArgs;
    }
    public override void Clear()
    {
        dialogueName = default;
        onCompleted = default;
    }
}
