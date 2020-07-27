using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int itemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
        itemCount = 0;
    }

    public void Add(T item)
    {
        item.HeapIndex = itemCount;
        items[itemCount] = item;
        SortUp(item);
        itemCount++;
    }

    public T RemoveTop()
    {
        //returns the top of the heap

        T top = items[0];
        itemCount--;
        items[0] = items[itemCount];
        items[0].HeapIndex = 0;

        SortDown(items[0]);

        return top;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get { return itemCount; }
    }


    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    private void SortUp(T item)
    {
        //sorts the item up the heap

        int parentIndex = (item.HeapIndex - 1) / 2;
        while(true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        //sorts item down the heap

        int swapIndex = 0;
        int child1Index;
        int child2Index;
        while (true)
        {
            child1Index = (item.HeapIndex * 2) + 1;
            child2Index = child1Index + 1;
            if (itemCount > child1Index)
            {
                swapIndex = child1Index;
                
                if (child2Index < itemCount)
                {
                    if (items[child1Index].CompareTo(items[child2Index]) < 0)
                    {
                        swapIndex = child2Index;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
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

    private void Swap(T item1, T item2)
    {
        //swaps two items in the heap
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;
        int item1Index = item1.HeapIndex;
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = item1Index;
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex {get; set;}
}