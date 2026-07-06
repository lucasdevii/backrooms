using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private List<Vector2Int> positions = new();

    public void AddTile(int x, int y)
    {
        positions.Add(new Vector2Int(x, y));
    }
}