using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
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
    public Vector2Int offsetCoor {get; set;}
    public Vector3 worldCoor {get; set;}
    public List<TileContainer> neighbors {get; set;}    
    private void Awake() {
        this.neighbors = new List<TileContainer>();
    }
}