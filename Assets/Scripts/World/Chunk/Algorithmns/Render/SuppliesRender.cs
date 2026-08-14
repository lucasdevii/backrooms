using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class SuplliesRender
{
    public static void Generate(Cell[,] matriz)
    {
        for (int y = 0; y < matriz.GetLength(0); y++)
        {
            for (int x = 0; x < matriz.GetLength(1); x++)
            {
                Cell cell = matriz[x, y].GetCell();

                foreach (Supply item in cell.suppliesObject)
                {
                    GameObject.Instantiate(item, item.position, Quaternion.identity);
                }
            }
        }
    }
}