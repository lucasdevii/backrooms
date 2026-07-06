using System.Collections.Generic;
using UnityEngine;
public static class ChunkDataGenerator
{
    public static void Generate(Cell[,] matriz, ulong seed, Vector2Int position)
    {
        List<Vector2Int> selectedRooms = new List<Vector2Int>();

        List<Room> chunkRoomsList = new List<Room>(RoomServices.GenerateRooms(matriz, seed, position));

    }


    
}
// Generate()
// ↓
// Descobre quantas salas
// ↓
// Pega as coordenadas das salas escolhidas
// ↓
// Expande as salas
// ↓
// Conecta as salas
// ↓
// Gera paredes