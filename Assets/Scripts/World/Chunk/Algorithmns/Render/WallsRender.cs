using System.Collections.Generic;
using UnityEngine;

public static class WallsRender
{
    public static void Render(
        GameObject chunkObject,
        int cellSize,
        Wall wallPrefab,
        Vector2 cellPosition,
        Cell cell
    )
    {
        HashSet<WorldManager.Direction> openedWalls = cell.GetOpenedWalls();

        // Parede direita
        if (!openedWalls.Contains(WorldManager.Direction.Right))
        {
            Wall wall = GameObject.Instantiate(
                wallPrefab,
                chunkObject.transform
            );

            wall.Inicialize(
                new Vector2(
                    cellPosition.x + (cellSize / 2f),
                    cellPosition.y
                ),
                cellSize + Wall.thickness,
                WorldManager.wallHeight,
                90
            );
        }

        // Parede inferior
        if (!openedWalls.Contains(WorldManager.Direction.Bottom))
        {
            Wall wall = GameObject.Instantiate(
                wallPrefab,
                chunkObject.transform
            );

            wall.Inicialize(
                new Vector2(
                    cellPosition.x,
                    cellPosition.y - (cellSize / 2f)
                ),
                cellSize + Wall.thickness,
                WorldManager.wallHeight,
                0
            );
        }
    }
}