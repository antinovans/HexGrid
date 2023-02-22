using System.Collections;
using System.Collections.Generic;

// public abstract class HexAStarComparer : Comparer<HexTile>
// {
//     protected HexTile start;
//     protected HexTile des;
//     public void SetStart(HexTile tile)
//     {
//         start = tile;
//     }
//     public void SetDes(HexTile tile)
//     {
//         des = tile;
//     }
// }
public class SmallHeuriticFirst : Comparer<(HexTile, int)>
{
    public override int Compare((HexTile, int) x, (HexTile, int) y)
    {
        if(x.Item2 > y.Item2)
        {
            return 1;
        }
        else if(x.Item2 < y.Item2)
            return -1;
        return 0;
    }
}
// public class LargeHeuriticFirst :HexAStarComparer
// {
//     public override int Compare(HexTile x, HexTile y)
//     {
//         if(x.fCost < y.fCost)
//             return 1;
//         else if(x.fCost > y.fCost)
//             return -1;
//         return 0;
//     }
// }
