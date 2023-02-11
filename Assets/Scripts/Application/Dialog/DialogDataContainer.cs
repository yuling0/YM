using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu(fileName = "New Dialog Data Container",menuName = "YM/DialogSystem")]
public class DialogDataContainer : NodeGraph
{
    public DialogNodeBase root;

    public override Node AddNode(Type type)
    {
        Node node = base.AddNode(type);
        if (root == null || root.position.x > node.position.x)
        {
            root = node as DialogNodeBase;
        }
        return node;
    }
}
