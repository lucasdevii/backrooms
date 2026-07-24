using Unity.Mathematics;
using UnityEngine;
public static class LampsRender
{
    public static void Render(        
        Chunk chunk,
        Vector2 chunkOrigin,
        Lamp lamp,
        GameObject chunkObject
    )
    {
        float heightOfLamps = WorldManager.wallHeight - (WorldManager.groundAndCeilingThickness / 2);;
        for (int row = 0; row < chunk.GetData().GetLength(0); row++)
        {
            float currentCellY = chunkOrigin.y - (row * WorldManager.cellSize) - WorldManager.cellSize;
            
            for (int col = 0; col < chunk.GetData().GetLength(1); col++)
            {
                float currentCellX = chunkOrigin.x + (col * WorldManager.cellSize) + WorldManager.cellSize;
                
                bool hasLight = chunk.GetCell(col, row).hasLight;

                Lamp newLamp = GameObject.Instantiate(lamp);
                newLamp.Inicialize(new Vector3(currentCellX, heightOfLamps, currentCellY), hasLight, chunkObject);
            }
        }
    }

    // public static void RenderLight(Chunk chunk, Vector2 chunkOrigin, GameObject lamp, GameObject chunkObject, )
    // {
    //     Vector3 positionForRender = new Vector3(
    //         chunkOrigin + WorldManager.cellSize * 

            
            
    //     );
    // }
}