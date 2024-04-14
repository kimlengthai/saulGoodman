using System;
using System.Collections;
using System.Collections.Generic;


public class PriorityQueue<T, U> : IEnumerable<T> where U : IComparable<U>
{
    List<T> elements = new List<T>();
    List<U> priorities = new List<U>();


    public int Count
    {
        get { return elements.Count; }
    }


    public void Enqueue(T element, U priority)
    {
        for (int i = 0; i < priorities.Count; i++)
        {
            if (priority.CompareTo(priorities[i]) > 0)
            {
                elements.Insert(i, element);
                priorities.Insert(i, priority);
                return;
            }
        }

        elements.Add(element);
        priorities.Add(priority);
    }


    public T Dequeue()
    {
        if (elements.Count == 0)
            return default(T);

        T element = elements[0];
        elements.RemoveAt(0);
        priorities.RemoveAt(0);
        return element;
    }


    public T Peek()
    {
        if (elements.Count == 0)
            return default(T);

        return elements[0];
    }


    public void Clear()
    {
        elements.Clear();
        priorities.Clear();
    }


    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return elements.GetEnumerator();
    }


    public IEnumerator GetEnumerator()
    {
        return elements.GetEnumerator();
    }


    public PriorityQueue<T, U> Clone()
    {
        PriorityQueue<T, U> clone = new PriorityQueue<T, U>();

        for (int i = elements.Count - 1; i >= 0; i--)
            clone.Enqueue(elements[i], priorities[i]);

        return clone;
    }
}