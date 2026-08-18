using System;
using UnityEngine;

public class PlayerRaycast
{
    float maxDistance;
    LayerMask layer;
    MainCamera camera;

    public PlayerRaycast(float maxDistance, LayerMask layer, MainCamera camera)
    {
        this.maxDistance = maxDistance;
        this.layer = layer;
        this.camera = camera;
    }

    public GameObject VerifyRaycast()
    {
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, maxDistance))
        {
            GameObject detectedObject = hit.collider.gameObject; 
            
            if(detectedObject.CompareTag("Item")){
                Debug.Log("Olhou para um item!");

                return detectedObject;
            }
        }
         
        return null;
    }
}