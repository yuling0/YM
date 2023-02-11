using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// A星寻路的节点
    /// </summary>
    public class AStarNode : HeapItem<AStarNode>
    {
        public int x;       //在map中所在的行
        public int y;       //在map中所在的列
        public bool isWalkable;
        public int g;       //起点到该点的消耗
        public int h;       //该点到目标点的预计消耗

        public int w;       //权重

        public Vector3 pos;
        public float posX;
        public float posY;

        public AStarNode parent;

        private int heapIndex;
        public int HeapIndex { get => heapIndex; set => heapIndex = value; }

        public AStarNode(int x, int y, int w, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.isWalkable = isWalkable;
        }

        public int F => g + h;  //预计的总消耗

        public int CompareTo(AStarNode other)
        {
            int res = F.CompareTo(other.F);
            if (res == 0)
            {
                res = h.CompareTo(other.h);
            }
            return -res;
        }
    }
}

