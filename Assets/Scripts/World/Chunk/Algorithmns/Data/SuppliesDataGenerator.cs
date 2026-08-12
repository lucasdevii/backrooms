using System;
using System.Collections.Generic;

public static class SuppliesDataGenerator
{
    public static void Generate(Cell[,] matriz, ulong chunkSeed, List<SupplyData> supplies)
    {
        for(int y = 0; y < matriz.GetLength(0); y++)
        {
            for (int x = 0; x < matriz.GetLength(1); x++)
            {
                float weight = 51.8f;
                
                float value = Noise.DefaultNoise(chunkSeed, x, y, weight);

                //A GERAÇÃO POR ENQUANTO SERÁ ALEATORIA, ENT DUAS PESSOAS PODEM VER ITENS DIFERENTES NA CELULA
                SupplyData supply = supplies[UnityEngine.Random.Range(0, supplies.Count)];
                
                matriz[x,y].SetSupply(supply);
            }
        }
    }
}