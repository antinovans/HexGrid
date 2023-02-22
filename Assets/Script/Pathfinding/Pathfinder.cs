using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public static class Pathfinder
{   
    private static Stack<HexTile> traces = new Stack<HexTile>();
    private static SmallHeuriticFirst cmpr = new SmallHeuriticFirst();
    private static Heap<(HexTile, int)> frontier = new Heap<(HexTile, int)>(cmpr);
    private static Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
    private static Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();
    public static Stack<HexTile> FindPath(HexTile start, HexTile des)
    {
        Debug.Assert(start != null);
        Debug.Assert(des != null);
        Debug.Assert(traces != null);
        Debug.Assert(frontier != null);
        Debug.Assert(cameFrom != null);
        Debug.Assert(costSoFar != null);
        //initialize
        traces.Clear();
        frontier.Clear();
        cameFrom.Clear();
        costSoFar.Clear();

        frontier.Add((start, 0));
        cameFrom[start] = null;
        costSoFar[start] = 0;

        while(!frontier.isEmpty())
        {
            var current = frontier.Pop();
            Debug.Assert(current.Item1 != null);

            if(current.Item1 == des)
                break;
            
            foreach(var next in current.Item1.neighbors)
            {
                if(next.isOccupied)
                    continue;
                int newcost = costSoFar[current.Item1] + 1;
                if(!costSoFar.ContainsKey(next) || newcost < costSoFar[next])
                {
                    costSoFar[next] = newcost;
                    int cost = newcost + Utils.HexPosDistance(des, next);
                    frontier.Add((next, cost));
                    cameFrom[next] = current.Item1;
                }
            }
        }
        //indicating we found a path
        if(cameFrom.ContainsKey(des))
        {
            traces.Push(des);
            var key = des;
            while(cameFrom[key] != null)
            {
                traces.Push(cameFrom[key]);
                key = cameFrom[key];
            }
            Debug.Assert(traces.Count == Utils.HexPosDistance(start,des));
        }
        return traces;
    }
}
