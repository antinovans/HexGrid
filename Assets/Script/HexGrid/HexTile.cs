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
    public Vector2Int offsetPos {get; set;}
    public Vector3 worldPos {get; set;}
    public List<HexTile> neighbors {get; set;}    
    public bool isOccupied;
    private void Awake() {
        this.neighbors = new List<HexTile>();
        isOccupied = false;
    }
    void Start()
    {
        
    }
    
    private void OnDisable() {
        
    }
}
