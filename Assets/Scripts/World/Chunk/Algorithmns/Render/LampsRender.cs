using UnityEngine;
public static class LampsRender
{
    public static void Render(        
        Chunk chunk,
        GameObject chunkObject,
        Vector2 chunkOrigin,
        int cellSize,
        int chunkSize,
        int wallHeight,
        Wall wallPrefab
    )
    {
        for (int row = 0; row < chunk.GetData().GetLength(0); row++)
        {
            for (int col = 0; col < chunk.GetData().GetLength(1); col++)
            {
                float value = Noise.DefaultNoise(chunk.chunkSeed, col, row);

                if(value < 0.2)
                {
                    //Vai ter luz nesse espaço
                    return;
                }
                //Não haverá luz
            }
        }
    }
}