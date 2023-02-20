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
    private HexRenderer hexRenderer;
    public bool isOccupied;
    private void Awake() {
        this.neighbors = new List<HexTile>();
        hexRenderer = GetComponent<HexRenderer>();
        isOccupied = false;
    }
    void Start()
    {
        EventManager.instance.onHighlightEvent += Rdr_Highlight;
        EventManager.instance.onNormalizeEvent += Rdr_Normalize;
        EventManager.instance.onRevertEvent += Rdr_Revert;
        EventManager.instance.onSwitchToConstructionEvent += Rdr_ShowAvailable;
        EventManager.instance.onConstructEvent += Rdr_Occupy;
    }
    public void Rdr_Revert(Vector2Int position)
    {
        if(offsetPos == position)
            hexRenderer.RevertMaterial();
    }
    public void Rdr_Normalize()
    {
        hexRenderer.SwitchMaterial(MaterialType.Normal);
    }
    public void Rdr_Highlight(Vector2Int position)
    {
        if(offsetPos == position)
            hexRenderer.SwitchMaterial(MaterialType.Highlight);
    }
    public void Rdr_Occupy(Vector2Int position)
    {
        if(offsetPos == position)
            hexRenderer.SwitchMaterial(MaterialType.Occupied);
    }
    public void Rdr_ShowAvailable()
    {
        if(isOccupied)
            hexRenderer.SwitchMaterial(MaterialType.Occupied);
        else
            hexRenderer.SwitchMaterial(MaterialType.Available);
    }
    private void OnDisable() {
        EventManager.instance.onHighlightEvent -= Rdr_Highlight;
        EventManager.instance.onNormalizeEvent -= Rdr_Normalize;
        EventManager.instance.onRevertEvent -= Rdr_Revert;
        EventManager.instance.onSwitchToConstructionEvent -= Rdr_ShowAvailable;
        EventManager.instance.onConstructEvent -= Rdr_Occupy;
    }
}
