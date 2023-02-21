using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapTest : MonoBehaviour
{
    Heap<int> heap;
    // Start is called before the first frame update
    void Start()
    {
        heap = new Heap<int>(Comparer<int>.Default);

        heap.Add(4);
        heap.Add(1);
        heap.Add(8);
        heap.Add(19);
        heap.Add(2);
        heap.Add(11);
        heap.Add(3);
        heap.Add(7);
        heap.Add(99);
        heap.Add(-2);
        heap.Add(5);
        heap.Add(21);
        heap.Add(81);
        while(!heap.isEmpty())
        {
            Debug.Log(heap.Pop());
        }
    }
}
