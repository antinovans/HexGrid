using System.Collections;
using System.Collections.Generic;

public class SmallHeuriticFirst :Comparer<HexTile>
{
    public override int Compare(HexTile x, HexTile y)
    {
        if(x.curHeuristic > y.curHeuristic)
            return 1;
        else if(x.curHeuristic < y.curHeuristic)
            return -1;
        return 0;
    }
}
public class LargeHeuriticFirst :Comparer<HexTile>
{
    public override int Compare(HexTile x, HexTile y)
    {
        if(x.curHeuristic < y.curHeuristic)
            return 1;
        else if(x.curHeuristic > y.curHeuristic)
            return -1;
        return 0;
    }
}
