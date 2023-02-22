using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderTest : MonoBehaviour
{
    public Vector2Int start;
    public Vector2Int end;
    // Start is called before the first frame update
    void Start()
    {
        var s = Pathfinder.FindPath(GridLayoutManager.instance.tiles[start], GridLayoutManager.instance.tiles[end]);
        Debug.Log(s.Count);
        // PrintStack(s);
    }

    private void PrintStack(Stack<HexTile> s)
    {
        while(!(s.Count != 0))
        {
            Debug.Log(s.Pop().offsetPos);
        }
    }
}
