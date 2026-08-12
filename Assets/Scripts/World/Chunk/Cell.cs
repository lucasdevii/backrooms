using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell
{
    private Lamp light;
    private ulong chunkSeed;
    private Vector2Int position;
    private Vector3 worldOrigin;
    private HashSet<WorldManager.Direction> openedWalls;
    private HashSet<Vector2Int> openedNeighboorsIndex;
    private List<SupplyData> supplies;

    public bool hasLight;
    private bool hasShadow = false;
 
    public Cell(ulong chunkSeed, int row, int col)
    {
        this.chunkSeed = chunkSeed;
        
        openedWalls = new HashSet<WorldManager.Direction>();
        openedNeighboorsIndex = new HashSet<Vector2Int>();

        position.x = col;
        position.y = row;
    }

    public Vector2Int GetPosition(){
        return position;
    }

    public void SetWorldOrigin(Vector3 origin)
    {
        worldOrigin = origin;
    }

    public Vector3 GetWorldOrigin()
    {
        return worldOrigin;
    }

    public Vector3 GetWorldCenter()
    {
        return worldOrigin + new Vector3(WorldManager.cellSize / 2f, 0f, -WorldManager.cellSize / 2f);
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
    public HashSet<Vector2Int> GetOpenedChunkNeighboorsIndex()
    {
        return openedNeighboorsIndex;
    }

    public void SetOpenedWalls(WorldManager.Direction wall){
        openedWalls.Add(wall);

        if(wall == WorldManager.Direction.Top) 
            openedNeighboorsIndex.Add(new Vector2Int(this.position.x, this.position.y + 1));

        if(wall == WorldManager.Direction.Bottom) 
            openedNeighboorsIndex.Add(new Vector2Int(this.position.x, this.position.y - 1)); 

        if(wall == WorldManager.Direction.Right) 
            openedNeighboorsIndex.Add(new Vector2Int(this.position.x + 1, this.position.y));

        if(wall == WorldManager.Direction.Left) 
            openedNeighboorsIndex.Add(new Vector2Int(this.position.x - 1, this.position.y));

    }

    public void OpenAllWalls()
    {
        openedWalls.Add(WorldManager.Direction.Top);
        openedWalls.Add(WorldManager.Direction.Bottom);
        openedWalls.Add(WorldManager.Direction.Left);
        openedWalls.Add(WorldManager.Direction.Right);

        if (position.y < WorldManager.cellsQuantityInChunk - 1)
            openedNeighboorsIndex.Add(new Vector2Int(position.x, position.y + 1));

        if (position.y > 0)
            openedNeighboorsIndex.Add(new Vector2Int(position.x, position.y - 1));

        if (position.x < WorldManager.cellsQuantityInChunk - 1)
            openedNeighboorsIndex.Add(new Vector2Int(position.x + 1, position.y));

        if (position.x > 0)
            openedNeighboorsIndex.Add(new Vector2Int(position.x - 1, position.y));
    }

    public void SetLightShadow(bool value)
    {
        hasShadow = value;
        
        if(light != null)
        {
            light.SetShadow(value);
        }
        
    }

    public void SetLightObject(Lamp light)
    {
        this.light = light;
    }

    public Lamp GetLightObject()
    {
        return light;
    }
    
    public void SetLampState(bool value)
    {
        if (light != null)
        {
            light.SetLight(value);
        }
    }

    public void SetSupply(SupplyData supply)
    {
        supplies.Add(supply);
    }
}
