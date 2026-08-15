using System;
using System.Collections.Generic;
using UnityEngine;

public static class SuppliesDataGenerator
{
    public static void Generate(Cell[,] matriz, ulong chunkSeed, List<SupplyData> supplies)
    {
        if (supplies == null || supplies.Count == 0)
        {
            Debug.LogError("Lista de suprimentos vazia");
            return;
        }

        
        for(int y = 0; y < matriz.GetLength(0); y++)
        {
            for (int x = 0; x < matriz.GetLength(1); x++)
            {
                


                Cell cell = matriz[x, y].GetCell();

                //Qual alimento será escolhido.
                float weightSupplySelect = 51.8f;
                
                float supplySelectionValue = Noise.DefaultNoise(chunkSeed, x, y, weightSupplySelect);

                SupplyData template = supplies[
                    Mathf.FloorToInt(
                        supplySelectionValue * supplies.Count
                    )
                ];

                SupplyData supply = UnityEngine.Object.Instantiate(template);

                //Posição do alimento.
                float weightSupplyPosition = 23.24f;

                Vector3 supplyPosition = new Vector3(
                    Noise.DefaultNoise(chunkSeed, x, y, weightSupplyPosition, 2) * supplies.Count,
                    Wall.thickness / 2 + 0.01f,
                    Noise.DefaultNoise(chunkSeed, x, y, weightSupplyPosition, 3) * supplies.Count
                );

                Vector3 result = cell.GetWorldOrigin() + supplyPosition;
                supply.SetPosition(result);
                
                cell.SetSupply(supply);
            }
        }
    }
}