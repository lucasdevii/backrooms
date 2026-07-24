using UnityEngine;

public static class ChunkRender
{
    public static void Render(
        Chunk chunk,
        Wall wallPrefab,
        int cellSize,
        int chunkSize,
        Vector3 groundAndCeilingSize,
        Lamp lamp
    )
    {
        Vector2 chunkOrigin = new Vector2(
            chunk.position.x * chunkSize,
            chunk.position.y * chunkSize + chunkSize
        );

        GameObject chunkObject = new GameObject(
            $"Chunk {chunk.position.x}, {chunk.position.y}"
        );

        chunk.SetChunkGameObject(chunkObject);

        float midOfChunk = chunkSize / 2f;

        GroundRender.Render(
            chunkOrigin,
            chunkObject,
            chunkSize,
            midOfChunk,
            groundAndCeilingSize
        );

        CeilingRender.Render(
            chunkOrigin,
            chunkObject,
            midOfChunk,
            groundAndCeilingSize
        );

        for (int row = 0; row < chunk.GetData().GetLength(1); row++)
        {
            for (int col = 0; col < chunk.GetData().GetLength(0); col++)
            {
                Vector2 cellPosition = new Vector2(
                    chunkOrigin.x + col * cellSize,
                    chunkOrigin.y - row * cellSize
                );

                Cell cell = chunk.GetData()[col, row];

                WallsRender.Render(
                    chunkObject,
                    cellSize,
                    wallPrefab,
                    cellPosition,
                    cell
                );
            }
        }

        //Vai carregar as lampadas em uma raio especifico
        LampsRender.Render(
            chunk,
            chunkOrigin,
            lamp,
            chunkObject
        );
    }

    public static void ConnectChunks(Chunk[,] matriz)
    {
        for (int row = 0; row < matriz.GetLength(0); row++)
        {
            for (int col = 0; col < matriz.GetLength(1); col++)
            {
                Chunk current = matriz[row, col];

                if (col + 1 < matriz.GetLength(1))
                    current.ConnectWith(
                        matriz[row, col + 1],
                        WorldManager.Direction.Right
                    );

                if (row + 1 < matriz.GetLength(0))
                    current.ConnectWith(
                        matriz[row + 1, col],
                        WorldManager.Direction.Bottom
                    );
            }
        }
    }
}