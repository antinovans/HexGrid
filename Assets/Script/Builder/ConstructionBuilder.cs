using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuilderMode
{
    Navigation,
    Selection,
    Build
}
public class ConstructionBuilder : MonoBehaviour
{
    Camera playerCam;
    GameObject constructionPrefab;
    HexTile prevTile;
    HexTile curTile;
    public BuilderMode curMode;
    private void Awake()
    {
        playerCam = GetComponent<Camera>();
        curMode = BuilderMode.Navigation;
        prevTile = null;
        curTile = null;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleBuildingMode();
        HandleTileRenderer();
        
    }

    private void HandleBuildingMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            curMode = BuilderMode.Build;
            EventManager.instance.ShowAllAvailableGrids();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            curMode = BuilderMode.Navigation;
            EventManager.instance.NormalizeMaterial();
        }
        if(curMode == BuilderMode.Build && Input.GetMouseButtonDown(0) && curTile!= null && !curTile.isOccupied)
        {
            EventManager.instance.BuildConstruction(curTile.offsetPos);
        }
    }

    void HandleTileRenderer()
    {
        var tile = CastRay();
        if(tile == prevTile)
            return;
        if (prevTile != null)
            EventManager.instance.RevertMaterial(prevTile.offsetPos);
        if (tile != null)
        {
            curTile = tile;
            EventManager.instance.HighlightMaterial(curTile.offsetPos);
            prevTile = curTile;
        }
    }
    HexTile CastRay()
    {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 3))
        {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.isTrigger)
            {
                var hitObj = hit.transform;
                hitObj.gameObject.TryGetComponent<HexTile>(out HexTile tile);
                if (tile != null)
                {
                    return tile;
                }
            }
        }
        return null;
    }
}
