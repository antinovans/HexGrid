using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapTest : MonoBehaviour
{
    Heap<(HexTile, int)> heap;
    private SmallHeuriticFirst cmpr = new SmallHeuriticFirst();
    // Start is called before the first frame update
    void Start()
    {
        
        while(!heap.isEmpty())
        {
            Debug.Log(heap.Pop().Item2);
        }
    }
}
