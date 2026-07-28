using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell
{
    private Lamp light;
    private ulong chunkSeed;
    private Vector2Int position;
    private HashSet<WorldManager.Direction> openedWalls;
    public bool hasLight;
    private bool hasShadow = false;
 
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

    public void OpenAllWalls()
    {
        openedWalls.Add(WorldManager.Direction.Top);
        openedWalls.Add(WorldManager.Direction.Bottom);
        openedWalls.Add(WorldManager.Direction.Left);
        openedWalls.Add(WorldManager.Direction.Right);
    }

    public void SetLightShadow(bool value)
    {
        hasShadow = value;
        light.SetShadow(value);
    }

    public void SetLightObject(Lamp light)
    {
        this.light = light;
    }

    public Lamp GetLightObject()
    {
        return light;
    }
}
