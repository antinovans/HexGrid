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
        int r = offsetCoor.x - (q - Mathf.Abs(q % 2)) / 2;
        return new Vector3Int(q, r, -q-r);
        // 1,0 =>0, 1, -1
        // 0,1 =>1, -1, 0
    }

    public static Vector2Int CubeToOffset(Vector3Int cubeCoor)
    {
        int col = cubeCoor.x;
        int row = cubeCoor.y + (cubeCoor.x - Mathf.Abs(cubeCoor.x % 2)) /2;
        return new Vector2Int(row, col);
    }
    public static Vector3Int HexPosAdd(HexTile t1, HexTile t2)
    {
        Vector3Int cubePos1 = OffsetToCube(t1.offsetPos);
        Vector3Int cubePos2 = OffsetToCube(t2.offsetPos);
        return new Vector3Int(cubePos1.x + cubePos2.x,
         cubePos1.y + cubePos2.y, cubePos1.z + cubePos2.z);
    }
    public static Vector3Int HexPosSubtract(HexTile t1, HexTile t2)
    {
        Vector3Int cubePos1 = OffsetToCube(t1.offsetPos);
        Vector3Int cubePos2 = OffsetToCube(t2.offsetPos);
        return new Vector3Int(cubePos1.x - cubePos2.x,
         cubePos1.y - cubePos2.y, cubePos1.z - cubePos2.z);
    }
    public static int HexPosDistance(HexTile t1, HexTile t2)
    {
        Vector3Int cubePos1 = OffsetToCube(t1.offsetPos);
        Vector3Int cubePos2 = OffsetToCube(t2.offsetPos);
        return (Mathf.Abs(cubePos1.x - cubePos2.x) +
                Mathf.Abs(cubePos1.y - cubePos2.y) + 
                Mathf.Abs(cubePos1.z - cubePos2.z))/2;
    }
    public static int HexPosDistance(Vector3Int cubePos1, Vector3Int cubePos2)
    {
        return (Mathf.Abs(cubePos1.x - cubePos2.x) +
                Mathf.Abs(cubePos1.y - cubePos2.y) + 
                Mathf.Abs(cubePos1.z - cubePos2.z))/2;
    }
}
