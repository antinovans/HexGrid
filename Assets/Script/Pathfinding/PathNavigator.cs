using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNavigator : MonoBehaviour
{
    public HexTile curTile {get; set;}
    private HexTile nextTile {get; set;}
    public HexTile destination {get; set;}
    private bool reachedCheckpoint = false;
    public float speed = 0.5f;
    private Stack<HexTile> _toVisit;
    private Stack<HexTile> _memory;
    private void Awake() {
        _toVisit = new Stack<HexTile>();
        _memory = new Stack<HexTile>();
    }   
    private void Start() {
        EventManager.instance.onConstructEvent += Reroute;
    }
    private void OnDisable() {
        EventManager.instance.onConstructEvent -= Reroute;
    }

    // Update is called once per frame
    void Update()
    {
        Navigate();
    }
    public void SetStartPosition(HexTile tile)
    {
        curTile = tile;
        nextTile = curTile;
        transform.position = tile.worldPos;
    }
    public void SetDestination(HexTile des)
    {
        destination = des;
        _toVisit = Pathfinder.FindPath(curTile, des);
    }
    private void Navigate()
    {
        //no tile to traverse
        if(_toVisit == null)
            return;
        //reach the destination
        if(_toVisit.Count == 0 && curTile == nextTile && reachedCheckpoint)
            return;
        //begin traverse to the first tile
        if(nextTile == null && _toVisit.Count != 0)
            nextTile = _toVisit.Pop();
        //move into next tile's territory
        if(Vector3.Distance(transform.position, nextTile.worldPos) < GridLayoutManager.DISTANCE_FROM_EDGE_TO_CENTER)
            curTile = nextTile;
        //exactly at the center of next tile
        if(Vector3.Distance(transform.position, nextTile.worldPos) < 0.01f)
        {
            reachedCheckpoint = true;
            if(_toVisit.Count > 0)
            {
                nextTile = _toVisit.Pop();
                reachedCheckpoint = false;
            }        
        }
        //moving 
        Move();
    }
    private void Move()
    {
        transform.LookAt(nextTile.worldPos);
        var step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, nextTile.worldPos, step);
    }
    private void Reroute(Vector2Int closedPos)
    {
        if(isPathClosed(closedPos))
            SetDestination(destination);
    }
    // s1     s2     s1
    //|5|    |1|    |5|
    //|4|    |2|    |4|
    //|3| => |3| => |3|
    //|2|    |4|    |2|
    //|1|    |5|    |1|
    private bool isPathClosed(Vector2Int closedPos)
    {
        while(_toVisit.Count != 0)
        {
            var top = _toVisit.Pop();
            if(top.offsetPos == closedPos)
            {
                _memory.Clear();
                return true;
            }
            _memory.Push(top);
        }
        while(_memory.Count != 0)
        {
            var top = _memory.Pop();
            _toVisit.Push(top);
        }
        return false;
    }
}
