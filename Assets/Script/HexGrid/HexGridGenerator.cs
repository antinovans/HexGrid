using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileContainer
{
    public TileContainer(Vector2Int offsetCoor, Vector3 worldCoor)
    {
        this.offsetCoor = offsetCoor;
        this.worldCoor = worldCoor;
        this.neighbors = new List<TileContainer>();
    }
    // public List<GameObject> content;
    public Vector2Int offsetCoor {get;}
    public Vector3 worldCoor {get;}
    public List<TileContainer> neighbors {get; set;}    
    public static Vector3Int[] neighborDirs = new Vector3Int[] {
        new Vector3Int(0, 1, -1),
        new Vector3Int(0, -1, 1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, 1, 0),
    };

   
}
public class HexGridGenerator : MonoBehaviour
{
    [Header("Layout setting")]
    public Vector2Int gridSize;
    public Vector2Int origin;

    [Header("Hex Grid Attribute")]
    [SerializeField]
    private float outerSize;
    [SerializeField]
    private float innerSize;
    [SerializeField]
    private float height;
    [SerializeField]
    public Material material;

    public Dictionary<Vector2Int, TileContainer> tileContainers;
    public static HexGridGenerator instance;

    private void OnEnable() {
        tileContainers = new Dictionary<Vector2Int, TileContainer>();
        GenerateLayout();
    }
    private void Awake() {
        instance = this;
        
    }
    // private void OnValidate() {
    //     if(Application.isPlaying)
    //         GenerateLayout();
    // }
    private void GenerateLayout()
    {
        //used to normalize the layout
        //make the central point (0,0)
        Vector2Int mid = new Vector2Int(gridSize.x/2, gridSize.y/2);
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPos = Coordinate2WorldPos(x - mid.x, y - mid.y);
                // GenerateGridMesh(x, y);
                GenerateGridMesh(x - mid.x, y - mid.y, worldPos);
                //initialize tile containers
                tileContainers.Add(new Vector2Int(x - mid.x, y - mid.y), 
                new TileContainer(new Vector2Int(x - mid.x, y - mid.y), worldPos
                ));
            }
        }

        //updating neighbors
        foreach(var container in tileContainers)
        {
            container.Value.neighbors = GetTileContainerNeighbor(container.Value);
            //testing
            // if(container.Key.x == 0)
            // {
            //     Debug.Log($"({container.Key.x},{container.Key.y})'s neighbors are:" );
            //     foreach(var ng in container.Value.neighbors)
            //     {
            //         Debug.Log($"{ng.offsetCoor.x}, {ng.offsetCoor.y}");
            //     }
            // }
        }
    }
    private void GenerateGridMesh(int x, int y, Vector3 worldPos)
    {
        GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
        var render = tile.GetComponent<HexRenderer>();
        render.outerRad = outerSize;
        render.innerRad = innerSize;
        render.hexHeight = height;
        render.SetHexMaterial(material);

        render.DrawHex();
        render.CombineHex();

        tile.transform.SetParent(transform, true);
        tile.transform.position = worldPos;
    }
    private Vector3 Coordinate2WorldPos(int x, int y)
    {
        int row = x;
        int col = y;
        float width;
        float height;
        float xPos;
        float zPos;
        bool shouldOffset;
        float horizontalDis;
        float verticalDis;
        float offset;
        float size = outerSize;

        shouldOffset =  Mathf.Abs(col) % 2 == 1;
        width = 2* size;
        height = Mathf.Sqrt(3) * size;

        horizontalDis = 3*width/ 4;
        verticalDis = height;

        offset = shouldOffset ? height/2 : 0;
        xPos = (col + origin.x) * horizontalDis;
        zPos = (row + origin.y) * verticalDis + offset;

        return new Vector3(xPos, 0, zPos);
    }
    private List<TileContainer> GetTileContainerNeighbor(TileContainer tileContainer)
    {
        List<TileContainer> neighbors = new List<TileContainer>();
        Vector3Int tileCubeCoor = Utils.OffsetToCube(tileContainer.offsetCoor);
        foreach(Vector3Int neighborDir in TileContainer.neighborDirs)
        {
            if(tileContainers.TryGetValue(Utils.CubeToOffset(tileCubeCoor + neighborDir), out TileContainer neighbor))
                neighbors.Add(neighbor);
        }
        return neighbors;
    }
}
