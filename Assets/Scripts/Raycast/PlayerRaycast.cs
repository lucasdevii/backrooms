using System;
using UnityEngine;

public class PlayerRaycast
{
    float maxDistance;
    LayerMask layer;
    MainCamera camera;
    float detectionRadius;

    public PlayerRaycast(float maxDistance, LayerMask layer, MainCamera camera, float detectionRadius)
    {
        this.maxDistance = maxDistance;
        this.layer = layer;
        this.camera = camera;
        this.detectionRadius = detectionRadius;
    }

    public GameObject VerifyRaycast()
    {
        Vector3 origin = camera.transform.position;
        Vector3 direction = camera.transform.forward;

        Debug.Log("Verificando raycast");
        if (
            Physics.SphereCast(
                origin,
                detectionRadius,
                direction,
                out RaycastHit hit,
                maxDistance,
                layer
            )
        )
        {
            GameObject detectedObject = hit.collider.gameObject; 
            Debug.Log("Detectei uma parada aqui hein" + detectedObject);
            
            if(detectedObject.CompareTag("Supply")){
                Debug.Log("Olhou para um item!");

                return detectedObject;
            }
        }
         
        return null;
    }
}