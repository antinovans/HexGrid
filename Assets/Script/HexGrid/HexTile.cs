using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public static Vector3Int[] neighborDirs = new Vector3Int[] {
        new Vector3Int(0, 1, -1),
        new Vector3Int(0, -1, 1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, 1, 0),
    };
    // public TileContainer(Vector2Int offsetCoor, Vector3 worldCoor)
    // {
    //     this.offsetCoor = offsetCoor;
    //     this.worldCoor = worldCoor;
    //     this.neighbors = new List<TileContainer>();
    // }
    // public List<GameObject> content;
    public Vector2Int offsetPos {get; set;}
    public Vector3 worldPos {get; set;}
    public List<HexTile> neighbors {get; set;}    
    private HexRenderer hexRenderer;
    private bool isOccupied;
    private void Awake() {
        this.neighbors = new List<HexTile>();
        hexRenderer = GetComponent<HexRenderer>();
    }
    void Start()
    {
        EventManager.instance.matHighlightEvent += OnRayEnter;
        EventManager.instance.matNormalizeEvent += OnRayExit;
    }
    public void OnRayExit(Vector2Int position)
    {
        if(offsetPos == position)
            hexRenderer.SwitchMaterial(MaterialType.Normal);
    }
    public void OnRayEnter(Vector2Int position)
    {
        if(offsetPos == position)
            hexRenderer.SwitchMaterial(MaterialType.Highlight);
    }
    private void OnDisable() {
        EventManager.instance.matHighlightEvent -= OnRayEnter;
        EventManager.instance.matNormalizeEvent -= OnRayExit;
    }
}
