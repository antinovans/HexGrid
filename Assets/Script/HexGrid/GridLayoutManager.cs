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
    public Dictionary<Vector2Int, HexRenderer> tileRenderers;
    public static GridLayoutManager instance;

    //global values
    public static float DISTANCE_FROM_EDGE_TO_CENTER;

    private void OnEnable()
    {
        tiles = new Dictionary<Vector2Int, HexTile>();
        tileRenderers = new Dictionary<Vector2Int, HexRenderer>();
        GenerateLayout();
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        DISTANCE_FROM_EDGE_TO_CENTER = outerSize * Mathf.Sqrt(3)/2;
    }
    private void Start()
    {
        EventManager.instance.onHighlightEvent += Rdr_Highlight;
        EventManager.instance.onNormalizeEvent += Rdr_Normalize;
        EventManager.instance.onRevertEvent += Rdr_Revert;
        EventManager.instance.onSwitchToConstructionEvent += Rdr_ShowAvailable;
        EventManager.instance.onConstructEvent += Rdr_Occupy;
        EventManager.instance.onConstructEvent += BuildConstruction;
    }
    private void OnDisable()
    {
        EventManager.instance.onHighlightEvent -= Rdr_Highlight;
        EventManager.instance.onNormalizeEvent -= Rdr_Normalize;
        EventManager.instance.onRevertEvent -= Rdr_Revert;
        EventManager.instance.onSwitchToConstructionEvent -= Rdr_ShowAvailable;
        EventManager.instance.onConstructEvent -= Rdr_Occupy;
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
        Vector2Int mid = new Vector2Int(gridSize.x / 2, gridSize.y / 2);
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
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
        foreach (var container in tiles)
        {
            container.Value.neighbors = GetTileNeighbor(container.Value);
        }
    }
    private void GenerateGridMesh(int x, int y, Vector3 worldPos)
    {
        Vector2Int pos = new Vector2Int(x, y);
        GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
        var hexRenderer = tile.GetComponent<HexRenderer>();
        tileRenderers.Add(pos, hexRenderer);
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
        hexTile.offsetPos = pos;
        hexTile.worldPos = worldPos;
        tiles.Add(pos, hexTile);

        tile.transform.SetParent(transform, true);
        tile.transform.position = worldPos;
    }
    //helper function for calculate the world position of each Hex Mesh
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

        shouldOffset = Mathf.Abs(col) % 2 == 1;
        width = 2 * size;
        height = Mathf.Sqrt(3) * size;

        horizontalDis = 3 * width / 4;
        verticalDis = height;

        offset = shouldOffset ? height / 2 : 0;
        xPos = (col + origin.x) * horizontalDis;
        zPos = (row + origin.y) * verticalDis + offset;

        return new Vector3(xPos, 0, zPos);
    }
    // Calculate neighbors of each tile
    private List<HexTile> GetTileNeighbor(HexTile tile)
    {
        List<HexTile> neighbors = new List<HexTile>();
        Vector3Int tileCubeCoor = Utils.OffsetToCube(tile.offsetPos);
        foreach (Vector3Int neighborDir in HexTile.neighborDirs)
        {
            if (tiles.TryGetValue(Utils.CubeToOffset(tileCubeCoor + neighborDir), out HexTile neighbor))
                neighbors.Add(neighbor);
        }
        return neighbors;
    }

    /*************events**************/
    /*↓↓↓↓↓ Tile related events ↓↓↓↓↓*/
    public void BuildConstruction(Vector2Int position)
    {
        tiles.TryGetValue(position, out HexTile tile);
        if (tile != null)
        {
            // Debug.Log($"Current tile {tile.offsetPos.x}, {tile.offsetPos.y}");
            var model = Instantiate(building, tile.worldPos, Quaternion.identity);
            model.transform.SetParent(tile.transform, true);
            tile.isOccupied = true;
            // foreach(var neighbor in tile.neighbors)
            // {
            //     Debug.Log($"neighbor tile {neighbor.offsetPos.x}, {neighbor.offsetPos.y}");
            // }
        }
    }
    /*↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑*/

    /*↓↓↓ Renderer related events ↓↓↓*/
    public void Rdr_Revert(Vector2Int position)
    {
        tileRenderers.TryGetValue(position, out HexRenderer hexRenderer);
        if (hexRenderer != null)
            hexRenderer.RevertMaterial();
    }
    public void Rdr_Normalize()
    {
        foreach (KeyValuePair<Vector2Int, HexRenderer> renderer in tileRenderers)
        {
            renderer.Value.SwitchMaterial(MaterialType.Normal);
        }
    }
    public void Rdr_Highlight(Vector2Int position)
    {
        tileRenderers.TryGetValue(position, out HexRenderer hexRenderer);
        if (hexRenderer != null)
            hexRenderer.SwitchMaterial(MaterialType.Highlight);
    }
    public void Rdr_Occupy(Vector2Int position)
    {
        tileRenderers.TryGetValue(position, out HexRenderer hexRenderer);
        if (hexRenderer != null)
            hexRenderer.SwitchMaterial(MaterialType.Occupied);
    }
    public void Rdr_ShowAvailable()
    {
        foreach (KeyValuePair<Vector2Int, HexTile> tile in tiles)
        {
            if (tile.Value.isOccupied)
            {
                tileRenderers.TryGetValue(tile.Value.offsetPos, out HexRenderer hexRenderer);
                hexRenderer?.SwitchMaterial(MaterialType.Occupied);
            }
            else
            {
                tileRenderers.TryGetValue(tile.Value.offsetPos, out HexRenderer hexRenderer);
                hexRenderer?.SwitchMaterial(MaterialType.Available);
            }
        }
    }
    /*↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑*/
}

