using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBuilder : MonoBehaviour
{
    Camera playerCam;
    HexTile curTile;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CastRay();
    }
    void CastRay()
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
                    if(curTile != tile)
                    {
                        if(curTile != null)
                            curTile.OnRayExit();
                        curTile = tile;
                        tile.OnRayEnter();
                    }
                        
                    // Debug.Log($"Hex {container.offsetCoor.x},{container.offsetCoor.y}");
                }
            }
        }
        else{
            if(curTile != null)
                curTile.OnRayExit();
            curTile = null;
        }
    }
}
