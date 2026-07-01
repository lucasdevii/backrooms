using System;
using System.Collections.Generic;
using UnityEngine;
public static class RoomPositionSelector
{

    //transforma o array em um array com coordenada das celulas selecionadas para serem salas.
    public static List<Vector2Int> GetRoomsCells(Cell[,] matriz, ulong seed, Vector2Int position)
    {
        List<Vector2Int> selectedCells = new List<Vector2Int>();

        float value = Noise.DefaultNoise(seed, position.x, position.y);
        int maxSize = matriz.GetLength(0);
        int numberOfRooms;
        
        //Se o value vir menor que 0.02 nenhuma sala será escolhida.
        if(value > 0.02 && value < 0.1) numberOfRooms = 1; // 1 SALA
        else if(value < 0.20) numberOfRooms = 2; // 2 SALAS
        else if(value < 0.40) numberOfRooms = 3; // 3 SALAS
        else if(value < 0.60) numberOfRooms = 4; // 4 SALAS
        else if(value < 0.80) numberOfRooms = 5; // 5 SALAS
        else if(value < 0.90) numberOfRooms = 6; // 6 SALAS
        else if(value < 0.95) numberOfRooms = 7; // 7 SALAS
        else numberOfRooms = 8; // 8 SALAS

        //Após determinar a quantidade de salas, busca as celulas para serem salas
        SelectRooms(selectedCells, matriz.GetLength(0), numberOfRooms, seed, position, value);

        return selectedCells;
    }

    private static void SelectRooms(
        List<Vector2Int> list, 
        int matrizSize,
        int numberOfRooms, 
        ulong seed, 
        Vector2Int position,
        float value
    )
    {
        for(int i = 0; i < numberOfRooms; i++){
            int row = Math.Ceiling(Noise.DefaultNoise(seed, position.x, position.y, value, 1) * matrizSize);
            int col = Math.Ceiling(Noise.DefaultNoise(seed, position.x, position.y, value, 2) * matrizSize);

            list.add();    
        }
    
        
    }
}