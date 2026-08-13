using System;
using System.Collections.Generic;
using UnityEngine;

public static class SuppliesDataGenerator
{
    public static void Generate(Cell[,] matriz, ulong chunkSeed, List<SupplyData> supplies)
    {
        for(int y = 0; y < matriz.GetLength(0); y++)
        {
            for (int x = 0; x < matriz.GetLength(1); x++)
            {
                //Qual alimento será escolhido.
                float weightSupplySelect = 51.8f;
                
                float supplySelectionValue = Noise.DefaultNoise(chunkSeed, x, y, weightSupplySelect);

                SupplyData supply = supplies[
                    Mathf.FloorToInt(
                        supplySelectionValue * supplies.Count
                    )
                ];

                //Posição do alimento.
                float weightSupplyPosition = 23.24f;

                Vector2 supplyPosition = new Vector2(
                    Noise.DefaultNoise(chunkSeed, x, y, weightSupplyPosition, 2) * supplies.Count,
                    Noise.DefaultNoise(chunkSeed, x, y, weightSupplyPosition, 3) * supplies.Count
                );

                supply.SetPosition(supplyPosition);
                
                matriz[x,y].SetSupply(supply);
            }
        }
    }
}