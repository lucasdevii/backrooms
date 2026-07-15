using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private ulong chunkSeed;
    private Vector2Int position;
    private HashSet<WorldManager.Direction> openedWalls;
    
    public Cell(ulong chunkSeed, int row, int col)
    {
        this.chunkSeed = chunkSeed;
        openedWalls = new HashSet<WorldManager.Direction>();
        position.x = col;
        position.y = row;
    }

    public Vector2Int GetPosition(){
        return position;
    }
    public ulong GetChunkSeed(){
        return chunkSeed;
    }
    public Cell GetCell()
    {
        return this;
    }
    public HashSet<WorldManager.Direction> GetOpenedWalls()
    {
        return openedWalls;
    }

    public void SetOpenedWalls(WorldManager.Direction wall){
        openedWalls.Add(wall);
    }
}
