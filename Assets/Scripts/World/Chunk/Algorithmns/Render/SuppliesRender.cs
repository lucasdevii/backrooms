using UnityEngine;

public static class SuplliesRender
{
    public static void GenerateInChunks(Chunk[,] matriz)
    {   
        for(int chunkY = 0; chunkY < matriz.GetLength(0); chunkY++)
        {
            for(int chunkX = 0; chunkX < matriz.GetLength(1); chunkX++)
            {
                Chunk chunk = matriz[chunkX, chunkY];
                Cell[,] internalGrid = chunk.GetData();

                Generate(internalGrid, chunk.GetChunkGameObject());
            }
        }
    }
    public static void Generate(Cell[,] matriz, GameObject chunk)
    {
        for (int y = 0; y < matriz.GetLength(0); y++)
        {
            for (int x = 0; x < matriz.GetLength(1); x++)
            {
                Cell cell = matriz[x, y].GetCell();

                foreach (SupplyData item in cell.suppliesData)
                {
                    if (item == null || item.prefab == null)
                        continue;

                    GameObject spawnedItem = Object.Instantiate(item.prefab, item.position, Quaternion.identity, chunk.transform);

                    Supply supplyComponent = spawnedItem.GetComponent<Supply>();

                    if (supplyComponent != null)
                    {
                        supplyComponent.Initialize(item);
                    }
                }
            }
        }
    }
}