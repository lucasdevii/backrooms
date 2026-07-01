using System.Collections.Generic;
using UnityEngine;
public static class ChunkDataGenerator
{
    public static Cell[,] Generate(Cell[,] matriz, ulong seed, Vector2Int position){
        List<Vector2> roomPoints = SelectRandomCells(matriz, seed, position);
        
        return 
    }


    //Retorna um array com coordenada das celulas selecionadas para serem salas.
    private static List<Vector2> SelectRandomCells(Cell[,] matriz, ulong seed, Vector2Int position){
        List<Vector2> 
        float value = Noise.DefaultNoise(seed, position.x, position.y);

        if(value < 0.1){
            
        }
        else if(value < 0.3)
        {
            
        }
        else if(value < 0.5)
        {
            
        }
        else if(value < 0.8)
        {
            
        }
        else
        {
            
        }
        return 
    }
}