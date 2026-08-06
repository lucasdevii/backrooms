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
        Vector3 cellOrigin = cell.GetWorldOrigin();

        // Parede direita
        if (!openedWalls.Contains(WorldManager.Direction.Right))
        {
            Wall wall = GameObject.Instantiate(
                wallPrefab,
                chunkObject.transform
            );

            wall.Inicialize(
                new Vector2(
                    cellOrigin.x + (cellSize / 2f),
                    cellOrigin.z
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
                    cellOrigin.x,
                    cellOrigin.z - (cellSize / 2f)
                ),
                cellSize + Wall.thickness,
                WorldManager.wallHeight,
                0
            );
        }
    }
}