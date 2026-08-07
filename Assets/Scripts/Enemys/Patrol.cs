using System;
using System.Collections.Generic;
using UnityEngine;
public static class Patrol
{
    /*
        Precisa pegar a posição atual.
        ESCOLHER UMA CASA PARA SE MOVER.
        Movimenta o player até a celula
        Verifica se o player esta no centro da celula
        Repete
    */
    public static List<Cell> GetValidNeighboors(Chunk chunk, Cell currentCell, List<Vector2Int> walkedCells)
    {
        //Pegar todos os caminhos validos
        //Pegar os caminhos ainda não visitados
        //Escolher uma celula dos caminhos possiveis com base em um noise
        
        HashSet<Vector2Int> possiblePaths = currentCell.GetOpenedChunkNeighboorsIndex();

        List<Cell> validPaths = new List<Cell>();

        foreach (Vector2Int path in possiblePaths)
        {
            if (!chunk.IsValidCell(path.x, path.y))
                continue;

            if (walkedCells.Contains(path))
                continue;

            validPaths.Add(chunk.GetCell(path.x, path.y));
        }

        return validPaths;
    }   

    public static Cell ChooseNextCell(List<Cell> validNeighboors, List<Vector2Int> walkedCells, Chunk chunk)
    {
        if(validNeighboors.Count == 0)
        {
            Vector2Int targetCell = walkedCells[walkedCells.Count - 1];

            return chunk.GetCell(targetCell.x, targetCell.y);
        }

        return validNeighboors[
            UnityEngine.Random.Range(0, validNeighboors.Count)
        ];
    }

    public static Cell GetNextCellForPatrol(
        Chunk chunk,
        Cell currentCell,
        List<Vector2Int> walkedCells,
        int maxHistory)
    {
        List<Cell> valid = GetValidNeighboors(chunk, currentCell, walkedCells);

        Cell next = ChooseNextCell(valid, walkedCells, chunk);

        UpdateHistory(walkedCells, currentCell, maxHistory);

        return next;
    }

    public static Vector3 GetDirection(Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }

    public static Vector3 GetVelocityDirection(Vector3 position, Cell targetCell, float moveSpeed)
    {
        //Pega a direção do centro da celula e aplica uma força para o inimigo ir até ela
        Vector3 targetPosition = targetCell.GetWorldCenter();
        Vector3 direction = GetDirection(position, targetPosition);

        return direction * moveSpeed;
    }

    public static bool VerifyOnTarget(Vector3 position, Cell targetCell)
    {
        Vector3 cellCenter = targetCell.GetWorldCenter();

        //Se tiver proximo o sulficiente retorna que é uma posição valida
        return Vector3.Distance(position, cellCenter) < 0.2f;
    }

    private static void UpdateHistory(List<Vector2Int> list, Cell newCell, int maxListSize)
    {
        if(list.Count >= maxListSize)
            list.RemoveAt(0);    

        list.Add(newCell.GetPosition());
    }
}