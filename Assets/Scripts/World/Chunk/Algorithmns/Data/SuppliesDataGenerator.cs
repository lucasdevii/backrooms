using System;
using System.Collections.Generic;
using UnityEngine;

public static class SuppliesDataGenerator
{
    public static float spawnChance = 0.01f;

    public static void Generate(Cell[,] matriz, ulong chunkSeed, List<SupplyData> supplies)
    {
        if (supplies == null || supplies.Count == 0)
        {
            Debug.LogError("Lista de suprimentos vazia");
            return;
        }

        for (int y = 0; y < matriz.GetLength(0); y++)
        {
            for (int x = 0; x < matriz.GetLength(1); x++)
            {
                Cell cell = matriz[x, y];

                // Gera no máximo um item por chunk para evitar centenas de suprimentos no mesmo ponto.
                float value = Noise.DefaultNoise(chunkSeed, x, y, 12.4f);
                if (value > spawnChance)
                    continue;

                //10% de chance
                float weightSupplySelect = 51.8f;
                float supplySelectionValue = Noise.DefaultNoise(chunkSeed, x, y, weightSupplySelect);

                int templateIndex = Mathf.Clamp(
                    Mathf.FloorToInt(supplySelectionValue * supplies.Count),
                    0,
                    supplies.Count - 1
                );

                SupplyData template = supplies[templateIndex];
                SupplyData supply = UnityEngine.Object.Instantiate(template);

                float offSetX = Math.Clamp(
                    Noise.DefaultNoise(chunkSeed, x, y, 2.3f) * WorldManager.cellSize, 
                    (Wall.thickness / 2) + 0.05f, 
                    WorldManager.cellSize - (Wall.thickness / 2) - 0.05f
                );
                
                float offSetZ = Math.Clamp(
                    Noise.DefaultNoise(chunkSeed, x, y, 8.9f) * WorldManager.cellSize, 
                    (Wall.thickness / 2) - 0.05f, 
                    WorldManager.cellSize + (Wall.thickness / 2) + 0.05f
                );

                Vector3 result = cell.GetWorldOrigin() + new Vector3(offSetX, 0.4f, offSetZ);

                supply.SetPosition(result);
                cell.SetSupply(supply);
            }
        }
    }

    public static void GenerateInChunks(Chunk[,] matriz, ulong chunkSeed, List<SupplyData> supplies)
    {
        if (supplies == null || supplies.Count == 0)
        {
            Debug.LogError("Lista de suprimentos vazia");
            return;
        }
        for (int chunkY = 0; chunkY < matriz.GetLength(0); chunkY++)
        {
            for(int chunkX = 0; chunkX < matriz.GetLength(1); chunkX++)
            {
                Generate(matriz[chunkX, chunkY].GetData(), chunkSeed, supplies);
            }
        }
        
    }
}