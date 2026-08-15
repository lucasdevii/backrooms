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

                foreach (SupplyData item in cell.suppliesData)
                {
                    if (item == null || item.prefab == null)
                        continue;

                    GameObject spawnedItem = Object.Instantiate(item.prefab, item.position, Quaternion.identity);
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