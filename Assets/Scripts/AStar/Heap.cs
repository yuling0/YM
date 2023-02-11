using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AStar
{
    //优先级队列 —— 堆
    public class Heap<T> where T : HeapItem<T>
    {
        public T[] arr;
        int count;
        public Heap(int size)
        {
            arr = new T[size];
            count = 0;
        }

        private bool HaveParent(int idx)
            => GetParentIndex(idx) == -1 ? false : true;

        private bool HaveLeftChild(int idx)
            => GetLeftChild(idx) < count ? true : false;

        private bool HaveRightChild(int idx)
            => GetRightChild(idx) < count ? true : false;


        private int GetParentIndex(int idx)
            => (idx - 1) >> 1;

        private int GetLeftChild(int idx)
            => (idx << 1) + 1;

        private int GetRightChild(int idx)
            => (idx << 1) + 2;


        public bool IsEmpty()
            => count <= 0;
        public void Add(T item)
        {
            if (count == arr.Length)
            {
                Debug.LogError("堆已满");
            }
            arr[count] = item;
            item.HeapIndex = count;
            HeapUp(item);
            count++;
        }

        public T Pop()
        {
            if (count == 0)
            {
                Debug.LogError("堆为空");
                return default(T);
            }
            T val = arr[0];

            Swap(arr[count - 1], arr[0]);

            count--;
            HeapDown(arr[0]);
            return val;
        }
        public bool Contains(T item)
        {
            if (item.HeapIndex < count)
            {
                if (arr[item.HeapIndex].Equals(item))
                    return true;
            }
            return false;
        }

        public void Update(T item)
        {
            HeapUp(item);

        }
        private void HeapUp(T Item)
        {
            int cur = Item.HeapIndex;
            int parentIndex;
            while (HaveParent(cur))
            {
                parentIndex = GetParentIndex(cur);

                if (arr[cur].CompareTo(arr[parentIndex]) > 0) //当前节点优先级更高
                {
                    Swap(arr[cur], arr[parentIndex]);
                    cur = parentIndex;
                }
                else
                {
                    break;
                }
            }

        }

        private void HeapDown(T Item)
        {
            int cur = Item.HeapIndex;
            int swapIndex;
            while (HaveLeftChild(cur))
            {
                swapIndex = GetLeftChild(cur);

                if (HaveRightChild(cur) && arr[swapIndex].CompareTo(arr[GetRightChild(cur)]) < 0)// 右节点的优先级高
                {
                    swapIndex = GetRightChild(cur);
                }

                if (arr[cur].CompareTo(arr[swapIndex]) < 0)
                {
                    Swap(arr[cur], arr[swapIndex]);
                    cur = swapIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private void Swap(T item1, T item2)
        {
            arr[item1.HeapIndex] = item2;
            arr[item2.HeapIndex] = item1;

            int temp = item1.HeapIndex;
            item1.HeapIndex = item2.HeapIndex;
            item2.HeapIndex = temp;
        }

    }

}

