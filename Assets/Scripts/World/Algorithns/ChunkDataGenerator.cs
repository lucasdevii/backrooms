using System.Collections.Generic;
using UnityEngine;

public static class ChunkDataGenerator
{
    public static void Generate(Cell[,] matrix, ulong seed, Vector2Int position)
    {
        int size = matrix.GetLength(0);

        HashSet<Vector2Int> visited = new();
        Stack<Vector2Int> path = new();

        Vector2Int current = Chunk.GetInitCell(seed, position, size);

        visited.Add(current);
        path.Push(current);

        while (path.Count > 0)
        {
            current = path.Peek();

            List<Vector2Int> unvisited = new();
            List<Vector2Int> visitedNeighbors = new();

            foreach (Vector2Int neighbor in GetValidNeighbors(current, size))
            {
                if (visited.Contains(neighbor))
                    visitedNeighbors.Add(neighbor);
                else
                    unvisited.Add(neighbor);
            }

            // Não existem mais caminhos para seguir.
            if (unvisited.Count == 0)
            {
                path.Pop();
                continue;
            }

            float noise = Noise.DefaultNoise(
                seed,
                current.x,
                current.y,
                path.Count,
                visited.Count
            );

            // Chance de criar uma conexão extra (loop).
            if (visitedNeighbors.Count > 0 && noise < 0.1f)
            {
                int index = Mathf.FloorToInt(
                    Noise.DefaultNoise(seed, current.x, current.y, 991)
                    * visitedNeighbors.Count
                );

                index = Mathf.Clamp(index, 0, visitedNeighbors.Count - 1);

                OpenPath(current, visitedNeighbors[index], matrix);
            }

            // Continua normalmente o DFS.
            int selected = Mathf.FloorToInt(noise * unvisited.Count);
            selected = Mathf.Clamp(selected, 0, unvisited.Count - 1);

            Vector2Int next = unvisited[selected];

            visited.Add(next);
            path.Push(next);

            OpenPath(current, next, matrix);
        }
    }

    public static void InitializeMatrix(Cell[,] matrix, ulong seed)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                matrix[y, x] = new Cell(seed, y, x);
            }
        }
    }

   static List<Vector2Int> GetValidNeighbors(Vector2Int cell, int size)
    {
        List<Vector2Int> neighbors = new();

        if (cell.x > 0)
            neighbors.Add(new Vector2Int(cell.x - 1, cell.y));

        if (cell.x < size - 1)
            neighbors.Add(new Vector2Int(cell.x + 1, cell.y));

        if (cell.y > 0)
            neighbors.Add(new Vector2Int(cell.x, cell.y - 1));

        if (cell.y < size - 1)
            neighbors.Add(new Vector2Int(cell.x, cell.y + 1));

        return neighbors;
    }

   static void OpenPath(Vector2Int position1, Vector2Int position2, Cell[,] matrix)
    {
        int dx = position2.x - position1.x;
        int dy = position2.y - position1.y;

        if(dx == 1 && dy == 0)//Se o movimento for para a direita
        {
            matrix[position1.y, position1.x].SetOpenedWalls(Cell.Direction.Right);
            matrix[position2.y, position2.x].SetOpenedWalls(Cell.Direction.Left);
        }
        else if(dx == -1 && dy == 0)//Se o movimento for para a esquerda
        {
            matrix[position1.y, position1.x].SetOpenedWalls(Cell.Direction.Left);
            matrix[position2.y, position2.x].SetOpenedWalls(Cell.Direction.Right);
        }
        else if(dx == 0 && dy == 1)//Se o movimento for para cima
        {
            matrix[position1.y, position1.x].SetOpenedWalls(Cell.Direction.Top);
            matrix[position2.y, position2.x].SetOpenedWalls(Cell.Direction.Bottom);
        }
        else if(dx == 0 && dy == -1)//Se o movimento for para baixo
        {
            matrix[position1.y, position1.x].SetOpenedWalls(Cell.Direction.Bottom);
            matrix[position2.y, position2.x].SetOpenedWalls(Cell.Direction.Top);
        }
    }
}