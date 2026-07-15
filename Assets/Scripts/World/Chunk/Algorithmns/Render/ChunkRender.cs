using UnityEngine;
public static class ChunkRender
{
    public static void Render(        
        Chunk chunk,
        GameObject chunkObject,
        int cellSize,
        int chunkSize,
        int wallHeight,
        Wall wallPrefab
    )
    {
    GroundRender.Render(        
        chunk,
        chunkObject,
        cellSize,
        chunkSize,
        wallHeight,
        wallPrefab
    );
    CeilingRender.Render(        
        chunk,
        chunkObject,
        cellSize,
        chunkSize,
        wallHeight,
        wallPrefab
    );
    WallsRender.Render(        
        chunk,
        chunkObject,
        cellSize,
        chunkSize,
        wallHeight,
        wallPrefab
    );
    LampsRender.Render(        
        chunk,
        chunkObject,
        cellSize,
        chunkSize,
        wallHeight,
        wallPrefab
    );
    }

    
}