using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderTest : MonoBehaviour
{
    public PathNavigator navigator;
    public Vector2Int start;
    public Vector2Int end;
    // Start is called before the first frame update
    void Start()
    {
        navigator.SetStartPosition(GridLayoutManager.instance.tiles[start]);
        navigator.SetDestination(GridLayoutManager.instance.tiles[end]);
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
