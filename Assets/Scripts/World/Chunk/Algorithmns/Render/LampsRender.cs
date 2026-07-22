using UnityEngine;
public static class LampsRender
{
    public static void Render(        
        Chunk chunk,
        Vector2 chunkOrigin
    )
    {
        for (int row = 0; row < chunk.GetData().GetLength(0); row++)
        {
            float currentCellY = chunkOrigin.y - (row * WorldManager.cellSize) - WorldManager.cellSize / 2;
            
            for (int col = 0; col < chunk.GetData().GetLength(1); col++)
            {
                float currentCellX = chunkOrigin.x + (col * WorldManager.cellSize) + WorldManager.cellSize / 2;
                
                bool hasLight = chunk.GetCell(col, row).hasLight;

                if (hasLight)
                {
                    //Intancia na posição currentCellX e currentCellY    
                }
            }
        }
    }
}