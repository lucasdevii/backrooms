using UnityEngine;
public static class GroundRender
{
    public static void Render(        
        Vector2 currentChunkOrigin, 
        GameObject chunkObject,
        float chunkSize,
        float midOfChunk,
        Vector3 groundAndCeilingSize
    )
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);

        ground.transform.position = new Vector3(
            currentChunkOrigin.x + midOfChunk, 
            0, 
            currentChunkOrigin.y - midOfChunk
        );
        ground.transform.localScale = new Vector3(
            groundAndCeilingSize.x, 
            groundAndCeilingSize.y, 
            groundAndCeilingSize.z
        );
        
        ground.transform.SetParent(chunkObject.transform);

    }
}