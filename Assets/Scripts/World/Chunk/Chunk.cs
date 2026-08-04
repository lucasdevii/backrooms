using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk
{
    // Removido o 'private' e adicionado 'public' com 'protected set'
    public ulong worldSeed { get; protected set; }
    public ulong seed { get; protected set; } 
    public int cellsQuantity { get; protected set; } // Também transformada em propriedade de leitura
    public Vector2Int position { get; protected set; }
    public Cell[,] internalGrid; 
    private GameObject chunkGameObject; // Referência ao GameObject do chunk na cena

    public Chunk(ulong worldSeed, int chunkX, int chunkZ)
    {
        position = new Vector2Int(chunkX, chunkZ);

        this.worldSeed = worldSeed;
        this.cellsQuantity = WorldManager.cellsQuantityInChunk;
        internalGrid = new Cell[cellsQuantity, cellsQuantity];

        seed = Generateseed();

        ChunkDataGenerator.InitializeMatrix(internalGrid, seed);
        ChunkDataGenerator.Generate(internalGrid, seed, position);
    }

    
    /* 
        Função de hash que retorna uma seed para cada chunk que 
        manipula o binario dos  numeros para evitar qualquer semelhança 
        determinada por numeros proximos
    */
    ulong Generateseed()
    {
        seed = worldSeed;

        seed ^= (ulong)position.x * 0x9E3779B185EBCA87UL;
        seed ^= (ulong)position.y * 0xC2B2AE3D27D4EB4FUL;

        seed ^= seed >> 30;
        seed *= 0xBF58476D1CE4E5B9UL;
        seed ^= seed >> 27;
        seed *= 0x94D049BB133111EBUL;
        seed ^= seed >> 31;

        return seed;
    }

    public void ConnectWith(Chunk otherChunk, WorldManager.Direction direction)
    {
        //É int pois representa apenas a coordenada do eixo X ou Y, dependendo da direção
        List<int> pointOfConnections = new List<int>();
        
        int sizeOfChunk = internalGrid.GetLength(0);

        for(int i = 0; i < sizeOfChunk; i++)
        {
            float value = Noise.DefaultNoise(seed, position.x, position.y, i);

            if(value < 0.4f)
            {
                pointOfConnections.Add(i);
            }
                
        }

        if(pointOfConnections.Count == 0)
        {
            int randomIndex = Mathf.FloorToInt(Noise.DefaultNoise(seed, position.x, position.y) * sizeOfChunk);
            pointOfConnections.Add(randomIndex);
        }
        
        foreach(int point in pointOfConnections)
        {
            if(direction == WorldManager.Direction.Right)
            {
                internalGrid[point, sizeOfChunk - 1].SetOpenedWalls(WorldManager.Direction.Right);
                otherChunk.internalGrid[point, 0].SetOpenedWalls(WorldManager.Direction.Left);
                break;
            }
            else if(direction == WorldManager.Direction.Bottom)
            {
                internalGrid[sizeOfChunk - 1, point].SetOpenedWalls(WorldManager.Direction.Bottom);
                    otherChunk.internalGrid[0, point].SetOpenedWalls(WorldManager.Direction.Top);
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

    public void SetChunkGameObject(GameObject chunkGameObject)
    {
        this.chunkGameObject = chunkGameObject;
    }

    public GameObject GetChunkGameObject()
    {
        return chunkGameObject;
    }

    public List<Vector2Int> GetValidCellNeighboorsIndex(Vector2Int cellIndex)
    {
        if(cellIndex.x < 0 || cellIndex.x >= cellsQuantity || cellIndex.y < 0 || cellIndex.y >= cellsQuantity)
        {
            throw new ArgumentOutOfRangeException("A coluna ou linha da celula não existe");
        }

        Cell currentCell = internalGrid[cellIndex.x, cellIndex.y];

        HashSet<WorldManager.Direction> neighboors = currentCell.GetOpenedWalls();
        List<Vector2Int> validNeighboors = new List<Vector2Int>();

        // -------- Verificações se a célula existe no grid interno ---------

        if(cellIndex.x - 1 < 0 && neighboors.Contains(WorldManager.Direction.Left)) 
            validNeighboors.Add(new Vector2Int(cellIndex.x - 1, cellIndex.y)); 

        if(cellIndex.x + 1 >= cellsQuantity && neighboors.Contains(WorldManager.Direction.Right)) 
            validNeighboors.Add(new Vector2Int(cellIndex.x + 1, cellIndex.y));

        if(cellIndex.y - 1 < 0 && neighboors.Contains(WorldManager.Direction.Top)) 
            validNeighboors.Add(new Vector2Int(cellIndex.x, cellIndex.y - 1));

        if(cellIndex.y + 1 >= cellsQuantity && neighboors.Contains(WorldManager.Direction.Bottom)) 
            validNeighboors.Add(new Vector2Int(cellIndex.x, cellIndex.y + 1));

        return validNeighboors;
    }

    public bool IsValidCell(int x, int y)
    {
        if(x > internalGrid.GetLength(0)) return false;
        else if(x < 0) return false;

        if(y > internalGrid.GetLength(1)) return false;
        else if(y < 0) return false;

        return true;
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