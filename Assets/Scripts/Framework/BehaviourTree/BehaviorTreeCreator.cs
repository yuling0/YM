using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;
using YMFramework.BehaviorTreeEditor;

public class BehaviorTreeCreator : SingletonBase<BehaviorTreeCreator>
{
    private Dictionary<string, TreeData> treeDatas = new Dictionary<string, TreeData>();
    private readonly string treeDataDirectory = "AI/";
    private string extension = ".ling";
    public Root CreateTree(string fileName,string extension = null)
    {
        if (extension != null)
        {
            this.extension = extension;
        }

        if (!treeDatas.ContainsKey(fileName))
        {
            string filePath = treeDataDirectory + fileName + this.extension;
            TreeData data = BinaryDataManager.Instance.LoadData<TreeData>(filePath);
            if (data != null)
            {
                treeDatas.Add(fileName, data);
            }
        }


        //生成行为树（写的很烂）
        TreeData treeData = treeDatas[fileName];
        Dictionary<int, Node> dic = new Dictionary<int, Node>();
        foreach (var nodeData in treeData.data)
        {
            Node node = null;
            switch (nodeData.NodeType)
            {
                case NodeDataBase.E_NodeType.Composite:
                    node = nodeData.CreateComposite();
                    break;
                case NodeDataBase.E_NodeType.Decorator:
                    node = nodeData.CreateDecorator();
                    break;
                case NodeDataBase.E_NodeType.Task:
                    node = nodeData.CreateTask();
                    break;
            }
            if (node == null)
            {
                Debug.LogError($"{nodeData.GetType()}:该NodeData类未设置NodeType");
            }
            dic.Add(nodeData.id, node);
        }

        Root root = dic[0] as Root;     //头结点为Root

        foreach (var nodeData in treeData.data)
        {
            Node cur = dic[nodeData.id];
            foreach (var id in nodeData.childrenId)
            {
                switch (nodeData.NodeType)
                {
                    case NodeDataBase.E_NodeType.Composite:
                        (cur as Composite).AddChild(dic[id]);
                        break;
                    case NodeDataBase.E_NodeType.Decorator:
                        (cur as Decorator).SetDecorator(dic[id]);
                        break;
                }
            }
        }

        return root;
    }

    public Root CreateTree(TreeNodeContainer behaviorTree)
    {
        XN_Root root = behaviorTree.GetRoot();
        if (root == null) return null;
        Root result = root.CreateNode() as Root;
        Queue<XN_NodeBase> nodeQueue1 = new ();
        Queue<Node> nodeQueue2 = new (); 
        nodeQueue1.Enqueue(root);
        nodeQueue2.Enqueue(result);
        while (nodeQueue1.Count > 0)
        {
            XN_NodeBase curNode1 = nodeQueue1.Dequeue();
            Node curNode2 = nodeQueue2.Dequeue();
            if (curNode1 is XN_Composite)
            {
                XN_Composite composite1 = (curNode1 as XN_Composite);
                Composite composite2 = (curNode2 as Composite);
                composite1.children.Sort((n1, n2) => { return n1.position.x < n2.position.x ? -1 : 1; });
                foreach (var tempNode in composite1.children)
                {
                    nodeQueue1.Enqueue(tempNode);
                    Node newNode = tempNode.CreateNode();
                    nodeQueue2.Enqueue(newNode);
                    composite2.AddChild(newNode);
                }
            }
            else if(curNode1 is XN_Decorator)
            {
                XN_Decorator decorator1 = (curNode1 as XN_Decorator);
                Decorator decorator2 = (curNode2 as Decorator);
                nodeQueue1.Enqueue(decorator1.decoratee);
                Node newNode = decorator1.decoratee.CreateNode();
                nodeQueue2.Enqueue(newNode);
                decorator2.SetDecorator(newNode);
            }
        }

        return result;
    }

}
