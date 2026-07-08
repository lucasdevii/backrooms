using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public enum Direction { 
        Top, 
        Left, 
        Right, 
        Bottom 
    }

    private ulong chunkSeed;
    private Vector2Int position;
    private HashSet<Direction> openedWalls;
    
    public Cell(ulong chunkSeed, int row, int col)
    {
        this.chunkSeed = chunkSeed;
        openedWalls = new HashSet<Direction>();
        position.x = col;
        position.y = row;
    }

    public Vector2Int GetPosition(){
        return position;
    }
    public ulong GetChunkSeed(){
        return chunkSeed;
    }

    public void SetOpenedWalls(Direction wall){
        openedWalls.Add(wall);
    }
}
