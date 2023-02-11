using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class Map : MonoBehaviour
    {
        public LayerMask noWalkable;

        public LayerMask canWalkable;

        public bool displayWeight;
        AStarNode[,] nodes;
        public float mapWidth = 100;          //地图宽
        public float mapHeight = 100;         //地图高度
        public float gridSizeX = 1.5f;         //格子宽
        public float gridSizeY = 1.5f;         //格子高
        public int gridRowCount;        //格子行数
        public int gridColCount;        //格子列数
        public Vector3 halfExtents;

        public int obstaclePenalty = 10;     //障碍的惩罚值（权重）
        public List<LayerInfo> layerInfos;
        public List<AStarNode> path;
        public List<AStarNode> openList;
        public HashSet<AStarNode> closeList;
        public Dictionary<int, int> layerInfoDictionary;

        int minWeight = int.MaxValue;
        int maxWeight = int.MinValue;
        private void Awake()
        {
            Init();
        }
        void Init()
        {
            //初始化层级信息
            layerInfos = new List<LayerInfo>() { new LayerInfo()
        { layer = LayerMask.GetMask("Plane"), penalty = 10 }, new LayerInfo() { layer = LayerMask.GetMask("Road"), penalty = 0 } };

            layerInfoDictionary = new Dictionary<int, int>();
            for (int i = 0; i < layerInfos.Count; i++)
            {
                canWalkable |= layerInfos[i].layer;
                layerInfoDictionary.Add(Mathf.RoundToInt(Mathf.Log(layerInfos[i].layer, 2)), layerInfos[i].penalty);
            }
            noWalkable.value = LayerMask.GetMask("NoWalkable");

            //计算格子
            gridRowCount = Mathf.RoundToInt(mapHeight / gridSizeY);
            gridColCount = Mathf.RoundToInt(mapWidth / gridSizeX);
            nodes = new AStarNode[gridRowCount, gridColCount];

            halfExtents = new Vector3(gridSizeX / 2, 1, gridSizeY / 2);     //格子大小的一半

            Vector3 worldBottomLeft = transform.position - Vector3.right * (mapWidth / 2) - Vector3.forward * (mapHeight / 2);  //地图左下角位置

            //生成格子、计算格子信息
            for (int i = 0; i < gridRowCount; i++)
            {
                for (int j = 0; j < gridColCount; j++)
                {
                    Vector3 pos = worldBottomLeft + Vector3.right * (j * gridSizeX + halfExtents.x) + Vector3.forward * (i * gridSizeY + halfExtents.z);
                    bool isWalkable = !Physics.CheckBox(pos, halfExtents, Quaternion.identity, noWalkable);  //检测当前格子是否可行走（检测周围是否有碰撞器）

                    int w = obstaclePenalty;    //权重

                    if (isWalkable)
                    {
                        //根据层级设置权重
                        Ray ray = new Ray(pos + Vector3.up * 10, Vector3.down);
                        if (Physics.Raycast(ray, out RaycastHit hit, 100, canWalkable))
                        {
                            if (layerInfoDictionary.TryGetValue(hit.transform.gameObject.layer, out int penalty))
                            {
                                w = penalty;
                            }
                        }
                    }

                    nodes[i, j] = new AStarNode(i, j, w, isWalkable);
                    nodes[i, j].pos = pos;

                    if (w > maxWeight) maxWeight = w;
                    if (w < minWeight) minWeight = w;
                }
            }

            SmoothWeight(3);
        }

        public void SmoothWeight(int blurSize)    //方框模糊
        {
            int kernelSize = blurSize << 1 | 1;         //内核大小 必须是奇数  如：模糊大小为 1 ，则是 3X3的内核
            int kernelExtents = blurSize;               //内核中心距两边的大小，如3*3内核，内核中心距旁边为1
            int[,] horizontalPenalty = new int[gridRowCount, gridColCount];
            int[,] verticalPenalty = new int[gridRowCount, gridColCount];

            //初始化第一行，方便后续计算
            for (int y = 0; y < gridColCount; y++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    x = Mathf.Clamp(x, 0, kernelExtents);//这里kernelExtents为负值时，说明在边界之外，只需要加上边界值就行，如 x = -1 , 钳制为 0 代表重复 0行y列的值
                    horizontalPenalty[0, y] += nodes[x, y].w;

                }

                //计算后续每一行（这里只是为了避免遍历 ， 如 第0行1到3列的和 = 0到2列的和 - 0列的值 + 3列的值）
                for (int x = 1; x < gridRowCount; x++)
                {
                    //需要移除的索引
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridRowCount);

                    //需要增加的索引
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridRowCount - 1);


                    horizontalPenalty[x, y] = horizontalPenalty[x - 1, y] - nodes[removeIndex, y].w + nodes[addIndex, y].w;
                }
            }



            //计算第一列
            for (int x = 0; x < gridRowCount; x++)
            {
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    y = Mathf.Clamp(y, 0, kernelExtents);
                    verticalPenalty[x, 0] += horizontalPenalty[x, y];

                    nodes[x, 0].w = Mathf.RoundToInt(verticalPenalty[x, 0] / (kernelSize * kernelSize));

                    if (nodes[x, y].w > maxWeight) maxWeight = nodes[x, y].w;
                    if (nodes[x, y].w < minWeight) minWeight = nodes[x, y].w;
                }

                for (int y = 1; y < gridColCount; y++)
                {
                    //需要移除的索引
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridColCount);

                    //需要增加的索引
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridColCount - 1);


                    verticalPenalty[x, y] = verticalPenalty[x, y - 1] - horizontalPenalty[x, removeIndex] + horizontalPenalty[x, addIndex];

                    nodes[x, y].w = Mathf.RoundToInt((float)verticalPenalty[x, y] / (kernelSize * kernelSize));

                    if (nodes[x, y].w > maxWeight) maxWeight = nodes[x, y].w;
                    if (nodes[x, y].w < minWeight) minWeight = nodes[x, y].w;
                }
            }



            //for (int x = 0; x < gridRowCount; x++)
            //{
            //    for (int y = 0; y < gridColCount; y++)
            //    {
            //        nodes[x, y].w = Mathf.RoundToInt(verticalPenalty[x, y] / (kernelSize * kernelSize));
            //    }
            //}


        }
        public int MaxSize => gridRowCount * gridColCount;
        public AStarNode GetNodeFromPosition(Vector3 pos)
        {
            float percentX = (pos.x + mapWidth / 2) / mapWidth;
            float percentY = (pos.z + mapHeight / 2) / mapHeight;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt(percentX * (gridColCount - 1));
            int y = Mathf.RoundToInt(percentY * (gridRowCount - 1));
            if (x >= 0 && x < gridColCount && y >= 0 && y < gridRowCount)
            {
                return nodes[y, x];
            }
            Debug.LogError("未找到对应位置的节点");
            return null;
        }

        public AStarNode[] GetNeighborFromNode(AStarNode node)
        {
            List<AStarNode> tempList = new List<AStarNode>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int x = node.x + i;
                    int y = node.y + j;
                    if (x >= 0 && x < gridRowCount && y >= 0 && y < gridColCount)
                    {
                        tempList.Add(nodes[x, y]);
                    }
                }
            }
            return tempList.ToArray();
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(mapWidth, 1, mapHeight));
            if (nodes == null) return;

            for (int i = 0; i < gridRowCount; i++)
            {
                for (int j = 0; j < gridColCount; j++)
                {
                    Gizmos.color = Color.green;
                    if (!nodes[i, j].isWalkable) Gizmos.color = Color.red;

                    if (path != null && path.Contains(nodes[i, j])) Gizmos.color = Color.black;
                    if (closeList != null && closeList.Contains(nodes[i, j])) Gizmos.color = Color.gray;

                    if (displayWeight)
                    {
                        Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(minWeight, maxWeight, nodes[i, j].w));
                    }

                    Gizmos.DrawCube(nodes[i, j].pos, new Vector3(gridSizeX, 1, gridSizeY));
                }
            }
        }

    }

}
