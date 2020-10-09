using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int itemCount;

    public Heap(int _maxHeapSize)
    {
        items = new T[_maxHeapSize];
        itemCount = 0;
    }

    public void Add(T _item)
    {
        _item.HeapIndex = itemCount;
        items[itemCount] = _item;
        SortUp(_item);
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

    public void UpdateItem(T _item)
    {
        SortUp(_item);
    }

    public int Count
    {
        get { return itemCount; }
    }


    public bool Contains(T _item)
    {
        return Equals(items[_item.HeapIndex], _item);
    }

    private void SortUp(T _item)
    {
        //sorts the item up the heap

        int parentIndex = (_item.HeapIndex - 1) / 2;
        while(true)
        {
            T parentItem = items[parentIndex];
            if (_item.CompareTo(parentItem) > 0)
            {
                Swap(_item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (_item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T _item)
    {
        //sorts item down the heap

        int swapIndex = 0;
        int child1Index;
        int child2Index;
        while (true)
        {
            child1Index = (_item.HeapIndex * 2) + 1;
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

                if (_item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(_item, items[swapIndex]);
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

    private void Swap(T _item1, T _item2)
    {
        //swaps two items in the heap
        items[_item1.HeapIndex] = _item2;
        items[_item2.HeapIndex] = _item1;
        int item1Index = _item1.HeapIndex;
        _item1.HeapIndex = _item2.HeapIndex;
        _item2.HeapIndex = item1Index;
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex {get; set;}
}