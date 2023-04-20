using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

namespace HughPathFinding
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private T[] itemArray;
        private int curItemCount;
        public int HeapCount { get { return this.curItemCount; } }
        public Heap(int maxHeapSize)
        {
            itemArray = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = curItemCount;
            itemArray[curItemCount] = item;
            HeapSortUp(item);
            curItemCount++;
        }
        public T RemoveFirst()
        {
            T firstItem = itemArray[0];
            curItemCount--;
            itemArray[0] = itemArray[curItemCount];
            itemArray[0].HeapIndex = 0;
            HeapSortDown(itemArray[0]);
            return firstItem;
        }

        public void UpdateHeap(T item)
        {
            HeapSortUp(item);
        }

        public bool Contains(T item)
        {
            return Equals(itemArray[item.HeapIndex], item);
        }

        private void HeapSortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while (true)
            {
                T parentItem = itemArray[parentIndex];
                if (0 < item.CompareTo(parentItem))
                {
                    SwapItem(item, parentItem);
                }
                else
                {
                    break;
                }
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void HeapSortDown(T item)
        {
            while (true)
            {
                int childLeftIdx = item.HeapIndex * 2 + 1;
                int childRightIdx = item.HeapIndex * 2 + 2;
                int swapIdx = 0;

                if (childLeftIdx < curItemCount)
                {
                    swapIdx = childLeftIdx;
                    if (childRightIdx < curItemCount)
                    {
                        if (itemArray[childLeftIdx].CompareTo(itemArray[childRightIdx]) < 0)
                        {
                            swapIdx = childRightIdx;
                        }
                    }

                    if (item.CompareTo(itemArray[swapIdx]) < 0)
                    {
                        SwapItem(item, itemArray[swapIdx]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SwapItem(T itemA, T itemB)
        {
            itemArray[itemA.HeapIndex] = itemB;
            itemArray[itemB.HeapIndex] = itemA;
            int tempIdx = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = tempIdx;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}