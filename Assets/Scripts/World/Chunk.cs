using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk
{
    // Removido o 'private' e adicionado 'public' com 'protected set'
    public ulong worldSeed { get; protected set; }
    public ulong chunkSeed { get; protected set; } 
    public int cellsQuantity { get; protected set; } // Também transformada em propriedade de leitura
    public Vector2Int position { get; protected set; }
    public Cell[,] internalGrid; 

    public Chunk(ulong worldSeed, int chunkX, int chunkZ, int cellsQuantity)
    {
        position = new Vector2Int(chunkX, chunkZ);

        this.worldSeed = worldSeed;
        this.cellsQuantity = cellsQuantity;
        internalGrid = new Cell[cellsQuantity, cellsQuantity];

        chunkSeed = GenerateChunkSeed();

        ChunkDataGenerator.InitializeMatrix(internalGrid, chunkSeed);
        ChunkDataGenerator.Generate(internalGrid, chunkSeed, position);
    }

    
    /* 
        Função de hash que retorna uma seed para cada chunk que 
        manipula o binario dos  numeros para evitar qualquer semelhança 
        determinada por numeros proximos
    */
    ulong GenerateChunkSeed()
    {
        chunkSeed = worldSeed;

        chunkSeed ^= (ulong)position.x * 0x9E3779B185EBCA87UL;
        chunkSeed ^= (ulong)position.y * 0xC2B2AE3D27D4EB4FUL;

        chunkSeed ^= chunkSeed >> 30;
        chunkSeed *= 0xBF58476D1CE4E5B9UL;
        chunkSeed ^= chunkSeed >> 27;
        chunkSeed *= 0x94D049BB133111EBUL;
        chunkSeed ^= chunkSeed >> 31;

        return chunkSeed;
    }

    public void ConnectWith(Chunk otherChunk, Cell.Direction direction)
    {
        //É int pois representa apenas a coordenada do eixo X ou Y, dependendo da direção
        List<int> pointOfConnections = new List<int>();
        
        int sizeOfChunk = internalGrid.GetLength(0);

        for(int i = 0; i < sizeOfChunk; i++)
        {
            float value = Noise.DefaultNoise(chunkSeed, position.x, position.y, i);

            if(value < 0.4f)
            {
                pointOfConnections.Add(i);
            }
                
        }

        if(pointOfConnections.Count == 0)
        {
            int randomIndex = Mathf.FloorToInt(Noise.DefaultNoise(chunkSeed, position.x, position.y) * sizeOfChunk);
            pointOfConnections.Add(randomIndex);
        }
        
        foreach(int point in pointOfConnections)
        {
            if(direction == Cell.Direction.Right)
            {
                internalGrid[point, sizeOfChunk - 1].SetOpenedWalls(Cell.Direction.Right);
                otherChunk.internalGrid[point, 0].SetOpenedWalls(Cell.Direction.Left);
                break;
            }
            else if(direction == Cell.Direction.Bottom)
            {
                internalGrid[sizeOfChunk - 1, point].SetOpenedWalls(Cell.Direction.Bottom);
                    otherChunk.internalGrid[0, point].SetOpenedWalls(Cell.Direction.Top);
                    break;
            }
        }
    }

    public static Vector2Int GetInitCell(ulong seed, Vector2Int chunkPosition, int matrizSize, float weight = 1)
    {
        int xWeight = 1;
        int yWeight = 2;

        return new Vector2Int(
            Mathf.FloorToInt(
                Noise.DefaultNoise(
                    seed, chunkPosition.x, chunkPosition.y, weight, xWeight
                ) * matrizSize
            ), 
            Mathf.FloorToInt(
                Noise.DefaultNoise(
                    seed, chunkPosition.x, chunkPosition.y, weight,yWeight
                ) * matrizSize
            )
        );
    }

    public Cell[,] GetData()
    {
        return internalGrid;
    }

    public Cell GetCell(int row, int col)
    {
        if(row < 0 || row >= cellsQuantity || col < 0 || col >= cellsQuantity)
        {
            throw new ArgumentOutOfRangeException("A coluna ou linha da celula não existe");
        }

        return internalGrid[row, col].GetCell();
    }

}
// -------------- IDEIA ---------------
// Chunk
// │
// ├── Criar sementes
// │
// ├── Expandir salas
// │
// ├── Resolver colisões
// │
// ├── Encontrar grupos conectados
// │
// ├── Criar corredores entre grupos
// │
// ├── Gerar paredes
// │
// ├── Gerar piso
// │
// ├── Posicionar portas
// │
// ├── Adicionar objetos
// │
// └── Iluminação