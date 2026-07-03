using System;
using System.Collections.Generic;
using UnityEngine;

public static class RoomPositionSelector
{

    //transforma o array em um array com coordenada das celulas selecionadas para serem salas.
    public static List<Vector2Int> GetRoomsCells(Cell[,] matriz, ulong seed, Vector2Int chunkPosition)
    {
        List<Vector2Int> selectedCells = new List<Vector2Int>();

        float value = Noise.DefaultNoise(seed, chunkPosition.x, chunkPosition.y);
        int numberOfRooms;

        //Se o value vir menor que 0.02 nenhuma sala será escolhida.
        if(value < 0.02f) numberOfRooms = 0;
        else if(value < 0.10f) numberOfRooms = 1; // 1 SALA
        else if(value < 0.20f) numberOfRooms = 2; // 2 SALAS
        else if(value < 0.40f) numberOfRooms = 3; // 3 SALAS
        else if(value < 0.60f) numberOfRooms = 4; // 4 SALAS
        else if(value < 0.80f) numberOfRooms = 5; // 5 SALAS
        else if(value < 0.90f) numberOfRooms = 6; // 6 SALAS
        else if(value < 0.95f) numberOfRooms = 7; // 7 SALAS
        else numberOfRooms = 8; // 8 SALAS

        //Após determinar a quantidade de salas, busca as celulas para serem salas
        SelectRooms(selectedCells, matriz.GetLength(0), numberOfRooms, seed, chunkPosition);

        //Expande as salas
        ScalingRooms(matriz, selectedCells, seed);

        return selectedCells;
    }

    // ----------------------  Funções necessárias  ----------------

    private static void SelectRooms(
        List<Vector2Int> selectedCellsList,
        int matrizSize,
        int numberOfRooms,
        ulong seed,
        Vector2Int chunkPosition
    )
    {
        const float rowWeight = 938.4f;
        const float colWeight = 752.9f;

        int attempts = 0;

        for(int i = 0; i < numberOfRooms; i++)
        {
            
            int row = Mathf.Clamp(
                (int)(Noise.DefaultNoise(seed, chunkPosition.x, chunkPosition.y, rowWeight, i, attempts) * matrizSize),
                0,
                matrizSize - 1
            );

            int col = Mathf.Clamp(
                (int)(Noise.DefaultNoise(seed, chunkPosition.x, chunkPosition.y, colWeight, i, attempts) * matrizSize),
                0,
                matrizSize - 1
            );

            Vector2Int position = new Vector2Int(col, row);

            //Se essa posição ainda n foi utilizada, é válida.
            if (!selectedCellsList.Contains(position))
            {
                selectedCellsList.Add(position);
                attempts = 0;
            }
            else
            {
                attempts++;

                if (attempts >= 100) break;

                i--;
            }
        }
    }

    private static void ScalingRooms(Cell[,] matriz, List<Vector2Int> selectedCells, ulong chunkSeed)
    {
        int numberOfRooms = selectedCells.Count;

        if(numberOfRooms == 0)
            return;

        //Determina a partir da quantidade de salas, o quanto cada ponto pode expandir
        int maxCellsForRoom = GetMaxCellsForRoom(matriz, numberOfRooms);

        //Vai realizar uma sequencia de ifs para saber se o valor
        for(int currentRoom = 0; currentRoom < selectedCells.Count; currentRoom++)
        {
            float value = Noise.DefaultNoise(
                chunkSeed,
                currentRoom,
                selectedCells[currentRoom].x,
                selectedCells[currentRoom].y
            );

            int cellsForExpanded = 1;

            for(int i = 1; i <= maxCellsForRoom; i++)
            {
                float percentInThisCase = 1f / maxCellsForRoom;

                if(value >= percentInThisCase * (i - 1) &&
                   value < percentInThisCase * i)
                {
                    cellsForExpanded = i;
                    break;
                }
            }

            ResizeRoom(matriz, selectedCells[currentRoom], cellsForExpanded);
        }
    }

    // ---------------------  Funções auxiliares  -------------------

    //Função que mede a quantidade q uma sala pode expandir dado o tamanho da matriz e a quantidade de salas
    private static int GetMaxCellsForRoom(Cell[,] matriz, int numberOfRooms)
    {
        int chunkSize = matriz.GetLength(0);

        return (int)Math.Floor((float)chunkSize / numberOfRooms);
    }

    //Função que faz o ponto de room expandir
    private static void ResizeRoom(Cell[,] matriz, Vector2Int pointOfRoom, int cellsForExpanded)
    {
        int minX = Math.Clamp(pointOfRoom.x - cellsForExpanded, 0, matriz.GetLength(1) - 1);
        int maxX = Math.Clamp(pointOfRoom.x + cellsForExpanded, 0, matriz.GetLength(1) - 1);

        int minY = Math.Clamp(pointOfRoom.y - cellsForExpanded, 0, matriz.GetLength(0) - 1);
        int maxY = Math.Clamp(pointOfRoom.y + cellsForExpanded, 0, matriz.GetLength(0) - 1);  

        for(int y = minY; y <= maxY; y++){
            for (int x = minX; x <= maxX; x++){
                matriz[y, x].ChangeCellType(Cell.CellType.Room);
            }
        }
    }
}