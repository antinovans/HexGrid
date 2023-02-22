using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNavigator : MonoBehaviour
{
    private Stack<HexTile> toVisit;
    public HexTile curTile {get; set;}
    public HexTile nextTile {get; set;}
    public float speed = 0.5f;
    private void Awake() {
        toVisit = new Stack<HexTile>();
    }   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Navigate();
    }
    public void SetDestination(HexTile des)
    {
        toVisit = Pathfinder.FindPath(curTile, des);
    }
    private void Navigate()
    {
        if(toVisit.Count == 0 && curTile == nextTile)
            return;
        if(nextTile == null)
            nextTile = toVisit.Pop();
        if(Vector3.Distance(transform.position, nextTile.worldPos) < 0.01f)
        {
            curTile = nextTile;
            if(toVisit.Count > 0)
            {
                nextTile = toVisit.Pop();
            }        
        }
        //moving 
        Move();
    }
    public void Move()
    {
        var step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, nextTile.worldPos, step);
    }
    public void SetStartPosition(HexTile tile)
    {
        curTile = tile;
        nextTile = curTile;
        transform.position = tile.worldPos;
    }
    private void PrintStack()
    {
        while(toVisit.Count != 0)
        {
            Debug.Log(toVisit.Pop().offsetPos);
        }
    }
}
