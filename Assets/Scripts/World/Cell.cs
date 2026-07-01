using UnityEngine;

public class Cell
{
    private ulong chunkSeed;
    private Vector2Int position;
    
    public Cell(ulong chunkSeed, int row, int col)
    {
        this.chunkSeed = chunkSeed;
        position.x = col;
        position.y = row;
    }

    public Vector2Int GetPosition(){
        return position;
    }
    public ulong GetChunkSeed(){
        return chunkSeed;
    }
}
