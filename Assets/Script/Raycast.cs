using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    Camera cam;
    private void Start() {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 3))
            {
                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if(hit.collider.isTrigger)
                {
                    var hitObj = hit.transform;
                    hitObj.gameObject.TryGetComponent<HexTile>(out HexTile container);
                    if(container != null)
                    {
                        Debug.Log($"Hex {container.offsetCoor.x},{container.offsetCoor.y}");
                        return;
                    }
                }
            }
            else{
                Debug.Log("Hit nothing");
            }
        }
        
    }
}
