using UnityEngine;
public static class CeilingRender
{
    public static void Render(        
        Vector2 currentChunkOrigin, 
        GameObject chunkObject,
        float midOfChunk,
        Vector3 groundAndCeilingSize
    )
    {
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        ceiling.transform.position = new Vector3(
            currentChunkOrigin.x + midOfChunk, 
            WorldManager.wallHeight, 
            currentChunkOrigin.y - midOfChunk
        );
        ceiling.transform.localScale = new Vector3(
            groundAndCeilingSize.x, 
            groundAndCeilingSize.y, 
            groundAndCeilingSize.z
        );

        ceiling.transform.SetParent(chunkObject.transform);
    }
}