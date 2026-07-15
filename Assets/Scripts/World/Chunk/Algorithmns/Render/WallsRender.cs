using System;
using System.Collections.Generic;
using UnityEngine;
public static class WallsRender
{
    public static void Render(        
        GameObject chunkObject,
        int cellSize,
        int wallHeight,
        Wall wallPrefab,
        Vector2 cellPosition, 
        Cell cell
    )
    {
        HashSet<WorldManager.Direction> openedWalls = cell.GetOpenedWalls();
    
        if(!openedWalls.Contains(WorldManager.Direction.Right))
        {
            //Deixa as paredes como filhas do gameObject do chunk, para melhor organização na hierarquia
            Wall wall = GameObject.Instantiate(wallPrefab, chunkObject.transform);

            wall.Inicialize(
                new Vector2(cellPosition.x + (cellSize / 2), cellPosition.y), 
                cellSize, 
                wallHeight, 
                90
            );
        }
        if(!openedWalls.Contains(WorldManager.Direction.Bottom))
        {
            Wall wall = GameObject.Instantiate(wallPrefab, chunkObject.transform);
            
            wall.Inicialize(
                new Vector2(cellPosition.x, cellPosition.y - (cellSize / 2)), 
                cellSize, 
                wallHeight, 
                0
            );

        }
    }
    
}