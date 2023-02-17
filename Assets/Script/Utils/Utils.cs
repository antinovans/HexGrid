using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    //coordinate conversion
    public static Vector3Int OffsetToCube(Vector2Int offsetCoor)
    {
        //     +q
        //
        //      o    
        //+s         +r
        int q = offsetCoor.y;
        int r = offsetCoor.x - (offsetCoor.y + offsetCoor.y % 2) / 2;
        return new Vector3Int(q, r, -q-r);
        // 1,0 =>0, 1, -1
        // 0,1 =>1, -1, 0
    }

    public static Vector2Int CubeToOffset(Vector3Int cubeCoor)
    {
        int offsetY = cubeCoor.x;
        int offsetX = cubeCoor.y + (offsetY + offsetY % 2) /2;
        return new Vector2Int(offsetX, offsetY);
    }
}
