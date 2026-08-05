using UnityEngine;

public static class LampsRender
{
    public static void Render(
        Chunk chunk,
        Vector2 chunkOrigin,
        Lamp lamp,
        GameObject chunkObject
    )
    {
        float heightOfLamps = WorldManager.wallHeight - (WorldManager.groundAndCeilingThickness / 2);

        for (int x = 0; x < chunk.GetData().GetLength(0); x++)
        {
            float currentCellX = chunkOrigin.x + (x * WorldManager.cellSize);

            for (int y = 0; y < chunk.GetData().GetLength(1); y++)
            {
                float currentCellY = chunkOrigin.y - (y * WorldManager.cellSize);

                Cell cellOfLight = chunk.GetCell(x, y);
                bool hasLight = cellOfLight.hasLight;

                Lamp newLamp = Object.Instantiate(lamp);

                cellOfLight.SetLightObject(newLamp);

                newLamp.Inicialize(new Vector3(currentCellX, heightOfLamps, currentCellY), hasLight, chunkObject);
            }
        }
    }
}