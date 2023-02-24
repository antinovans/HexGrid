using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderTest : MonoBehaviour
{
    public PathNavigator navigator;
    public Vector2Int start;
    public Vector2Int end;
    public bool toggle = false;
    // Start is called before the first frame update
    void Start()
    {
        navigator.SetStartPosition(GridLayoutManager.instance.tiles[start]);
        navigator.SetDestination(GridLayoutManager.instance.tiles[end]);
        // PrintStack(s);
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            navigator.SetDestination(GridLayoutManager.instance.tiles[start]);
        }
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            navigator.SetDestination(GridLayoutManager.instance.tiles[end]);
        }
    }

    private void PrintStack(Stack<HexTile> s)
    {
        while(!(s.Count != 0))
        {
            Debug.Log(s.Pop().offsetPos);
        }
    }
}
