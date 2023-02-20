using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridLayoutManager : MonoBehaviour
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
    [Header("Hex Grid Renderer Attribute")]
    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material availableMaterial;
    [SerializeField]
    private Material occupiedMaterial;
    [Header("buildings prefabs")]
    [SerializeField]
    private GameObject building;
    

    public Dictionary<Vector2Int, HexTile> tiles;
    public static GridLayoutManager instance;

    private void OnEnable() {
        tiles = new Dictionary<Vector2Int, HexTile>();
        GenerateLayout();
    }
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Start() {
        EventManager.instance.onConstructEvent += BuildConstruction;
    }
    private void OnDisable() {
        EventManager.instance.onConstructEvent -= BuildConstruction;
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
                // tileContainers.Add(new Vector2Int(x - mid.x, y - mid.y), 
                // new TileContainer(new Vector2Int(x - mid.x, y - mid.y), worldPos
                // ));
            }
        }

        //updating neighbors
        foreach(var container in tiles)
        {
            container.Value.neighbors = GetTileContainerNeighbor(container.Value);
        }
    }
    private void GenerateGridMesh(int x, int y, Vector3 worldPos)
    {
        GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
        var hexRenderer = tile.GetComponent<HexRenderer>();
        hexRenderer.outerRad = outerSize;
        hexRenderer.innerRad = innerSize;
        hexRenderer.hexHeight = height;
        hexRenderer.normal_mat = idleMaterial;
        hexRenderer.hightlight_mat = highlightMaterial;
        hexRenderer.available_mat = availableMaterial;
        hexRenderer.occupied_mat = occupiedMaterial;
        hexRenderer.SwitchMaterial(MaterialType.Normal);
        //setting up renderer
        hexRenderer.DrawHex();
        hexRenderer.CombineHex();
        //setting up collider and layer for raycast
        MeshCollider mc = tile.AddComponent<MeshCollider>() as MeshCollider;
        mc.convex = true;
        mc.isTrigger = true;
        tile.layer = 3;
        //appending HexTile.cs on each tile
        HexTile hexTile = tile.AddComponent<HexTile>() as HexTile;
        hexTile.offsetPos = new Vector2Int(x, y);
        hexTile.worldPos = worldPos;
        tiles.Add(hexTile.offsetPos, hexTile);

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
    private List<HexTile> GetTileContainerNeighbor(HexTile tile)
    {
        List<HexTile> neighbors = new List<HexTile>();
        Vector3Int tileCubeCoor = Utils.OffsetToCube(tile.offsetPos);
        foreach(Vector3Int neighborDir in HexTile.neighborDirs)
        {
            if(tiles.TryGetValue(Utils.CubeToOffset(tileCubeCoor + neighborDir), out HexTile neighbor))
                neighbors.Add(neighbor);
        }
        return neighbors;
    }
    public void BuildConstruction(Vector2Int position)
    {
        tiles.TryGetValue(position, out HexTile tile);
        if(tile != null)
        {
            var model = Instantiate(building, tile.worldPos, Quaternion.identity);
            model.transform.SetParent(tile.transform, true);
            tile.isOccupied = true;
        }
    }
    
}

