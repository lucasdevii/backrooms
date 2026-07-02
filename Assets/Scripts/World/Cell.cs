using UnityEngine;

public class Cell
{
    enum Type
    {
        Room,
        Maze,
        Wall,
        Door
    }

    private ulong chunkSeed;
    private Vector2Int position;

    private Type type;
    
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
