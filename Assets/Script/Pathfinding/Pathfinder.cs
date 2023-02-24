using System.Collections;
using System.Collections.Generic;

public class Pathfinder
{   
    private static Stack<HexTile> traces = new Stack<HexTile>();
    private static SmallHeuriticFirst cmpr = new SmallHeuriticFirst();
    private static Heap<(HexTile, int)> frontier = new Heap<(HexTile, int)>(cmpr);
    private static Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
    private static Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();
    public static Stack<HexTile> FindPath(HexTile start, HexTile des)
    {
        //initialize
        traces.Clear();
        frontier.Clear();
        cameFrom.Clear();
        costSoFar.Clear();
        
        if(des.isOccupied == true)
            return null;

        frontier.Add((start, 0));
        cameFrom[start] = null;
        costSoFar[start] = 0;

        while(!frontier.isEmpty())
        {
            var current = frontier.Pop();

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
                    int fcost = newcost + Utils.HexPosDistance(des, next);
                    frontier.Add((next, fcost));
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
        }
        return traces;
    }
}
