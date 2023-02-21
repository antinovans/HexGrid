using System.Collections;
using System.Collections.Generic;
using System;
public class Heap<T>
{
    public List<T> m_heap;
    private Comparer<T> _comparer;

    public int Count => m_heap.Count;
    public Heap(Comparer<T> comparer)
    {
        m_heap = new List<T>();
        this._comparer = comparer;
    }
    public void Add(T value)
    {
        m_heap.Add(value);
        HeapifyUp(m_heap.Count - 1);
    }
    public T Top()
    {
        return m_heap[0];
    }
    public T Pop()
    {
        var top = Top();
        (m_heap[0], m_heap[m_heap.Count - 1]) = (m_heap[m_heap.Count - 1], m_heap[0]);
        m_heap.RemoveAt(m_heap.Count - 1);
        HeapifyDown(0);
        return top;
    }
    public bool isEmpty()
    {
        return m_heap.Count == 0;
    }
    private void HeapifyDown(int index)
    {
        int left = 2 * index + 1;
        int right = 2 * index + 2;
        int max = index;
        if(left < m_heap.Count && _comparer.Compare(m_heap[left], m_heap[max]) < 0)
            max = left;
        if(right < m_heap.Count && _comparer.Compare(m_heap[right], m_heap[max]) < 0)
            max = right;
        if(max != index)
        {
            (m_heap[index], m_heap[max]) = (m_heap[max], m_heap[index]);
            HeapifyDown(max);
        }
    }
    private void HeapifyUp(int index)
    {
        // Debug.Assert(index >=0 && index < m_heap.Count);
        if(index <= 0)
            return;
        int parent = (index - 1)/2;
        
        // Debug.Assert(parent >=0 && parent < m_heap.Count);
        if(_comparer.Compare(m_heap[index], m_heap[parent]) < 0)
        {
            (m_heap[index], m_heap[parent]) = (m_heap[parent], m_heap[index]);
            HeapifyUp(parent);
        }
    }
    private void Swap(int i, int j)
    {
        var t = m_heap[i];
        m_heap[i] = m_heap[j];
        m_heap[j] = t;
    }
}
