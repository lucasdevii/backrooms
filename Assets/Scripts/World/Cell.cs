using UnityEngine;

public class Cell
{
    public enum CellType
    {
        Room,
        Maze,
        Wall,
        Door
    }

    private ulong chunkSeed;
    private Vector2Int position;

    private CellType type;
    
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
    public void ChangeCellType(CellType newCellType){
        type = newCellType;
    }
}
