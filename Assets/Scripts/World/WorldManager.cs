using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public enum Direction { 
        Top, 
        Left, 
        Right, 
        Bottom 
    }

    [SerializeField] private Transform playerPosition;
    
    [SerializeField] private Wall wallPrefabScript;
    [SerializeField] private Lamp lamp;

    [SerializeField] public List<SupplyData> supplies;

    public static WorldManager Instance;
    
    public ulong seed = 4196283291932386;
 
    public Vector2Int playerChunk = new Vector2Int();
    public Vector2Int playerCell = new Vector2Int();

    public static int renderLightsRadius = 10;
    public int renderDistance = 1;
    public Chunk[,] matriz;

    //--------------- CHUNK ----------------,,
    public int chunkSize; //Tamanho de cada chunk em unidades de escala do game

    //-------------- CÉLULAs ---------------
    public static int cellSize = 5; 
    public static int cellsQuantityInChunk = 20;  

    public static float wallHeight = 6f;
    private Vector3 groundAndCeilingSize; //Precisa ter o mesmo tamanho que o chunkSize
    public static float groundAndCeilingThickness = 0.5f;

    //-------------- Events ----------------

    public bool isBlackout = false;
  

    void Awake()
    {
        if (Instance == null) Instance = this;
    
        int matrizSize = (renderDistance * 2) + 1;

        chunkSize = cellSize * cellsQuantityInChunk;

        matriz = new Chunk[matrizSize, matrizSize];
        groundAndCeilingSize = new Vector3(chunkSize, groundAndCeilingThickness, chunkSize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se o player n foi arrastado para o inspector, pega na cena
        if (playerPosition == null){
            playerPosition = GameObject.FindWithTag("Player").transform;
        }

        DefinePlayerChunk();
        DefinePlayerCell();

        FillInMatrizOfChunks();
        ChunkRender.ConnectChunks(matriz);
        InstantiateChunksInWorld();
        SuplliesRender.GenerateInChunks(matriz);

        StartCoroutine(BlackoutEvent.RollBlackoutChance());
    }

    // Update is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        VerifyIfChunkChange();
        VerifyIfCellChange();
    }

    void DefinePlayerChunk()
    {
        playerChunk.x = Mathf.FloorToInt(playerPosition.position.x / chunkSize);
        playerChunk.y = Mathf.FloorToInt(playerPosition.position.z / chunkSize);
    }

    void DefinePlayerCell()
    {
        Vector2Int chunkPlayerOrigin = new Vector2Int(playerChunk.x * chunkSize, playerChunk.y * chunkSize);
        
        int cellXInChunk = Mathf.FloorToInt(playerPosition.position.x - chunkPlayerOrigin.x);
        int cellYInChunk = Mathf.FloorToInt(playerPosition.position.z - chunkPlayerOrigin.y);

        playerCell.x = cellXInChunk;
        playerCell.y = cellYInChunk;
    }
    
    bool VerifyIfChunkChange()
    {
        if (playerPosition == null) return false;

        Vector2Int currentChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerPosition.position.x / chunkSize),
            Mathf.FloorToInt(playerPosition.position.z / chunkSize)
        );

        if (currentChunkPosition != playerChunk)
        {
            int dx = playerChunk.x - currentChunkPosition.x;
            int dy = playerChunk.y - currentChunkPosition.y;

            LoadNewChunks(dx, dy);
            playerChunk.x = currentChunkPosition.x;
            playerChunk.y = currentChunkPosition.y;
            return true;
        }

        playerChunk.x = currentChunkPosition.x;
        playerChunk.y = currentChunkPosition.y;
        return false;
    }

    bool VerifyIfCellChange()
    {
        if (playerPosition == null)
            return false;

        Vector2Int currentChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerPosition.position.x / chunkSize),
            Mathf.FloorToInt(playerPosition.position.z / chunkSize)
        );

        Vector2Int chunkOrigin = currentChunkPosition * chunkSize;

        Vector2Int currentCellPosition = new Vector2Int(
            Mathf.FloorToInt(playerPosition.position.x - chunkOrigin.x),
            Mathf.FloorToInt(playerPosition.position.z - chunkOrigin.y)
        );

        bool changed = currentCellPosition != playerCell;

        playerCell = currentCellPosition;
        return changed;
    }

    void FillInMatrizOfChunks()
    {
        //Retorna o indice de onde fica o jogador na matriz (no centro da renderização)
        int matrizCenter = matriz.GetLength(0) / 2;
        
        //Começa com as coordenadas do chunk superior esquerdo
        int initialChunkX = playerChunk.x - matrizCenter;
        int initialChunkY = playerChunk.y + matrizCenter;

        int currentChunkY = initialChunkY;
        
        for(int row = 0; row < matriz.GetLength(0); row++){

            int currentChunkX = initialChunkX;

            for(int col = 0; col < matriz.GetLength(1); col++){
                matriz[row, col] = new Chunk(seed, currentChunkX, currentChunkY);

                currentChunkX++;
            }
            
            currentChunkY--;
        }   
    }
    
    void InstantiateChunksInWorld()
    {
        for (int row = 0; row < matriz.GetLength(0); row++)
        {
            for (int col = 0; col < matriz.GetLength(1); col++)
            {
                Chunk chunk = matriz[row, col];

                ChunkRender.Render(
                    chunk,
                    wallPrefabScript,
                    cellSize,
                    chunkSize,
                    groundAndCeilingSize,
                    lamp
                );
            }
        }
    }

    void LoadNewChunks(int dx, int dy)
    {
        if(dx > 0) //Player se moveu para a esquerda
        {
            DestroyChunksInWorld(Direction.Right);
            ChunkDataGenerator.WalkingForTheRightChunk(matriz, seed);
            InstantiateNewChunksInWorld(Direction.Left);
        }
        else if(dx < 0) //Player se moveu para a direita
        {
            DestroyChunksInWorld(Direction.Left);
            ChunkDataGenerator.WalkingForTheLeftChunk(matriz, seed);
            InstantiateNewChunksInWorld(Direction.Right);
        }
        if(dy > 0) //Player se moveu para baixo
        {
            DestroyChunksInWorld(Direction.Top);
            ChunkDataGenerator.WalkingForTheTopChunk(matriz, seed);
            InstantiateNewChunksInWorld(Direction.Bottom);
        }
        else if(dy < 0) //Player se moveu para cima
        {
            DestroyChunksInWorld(Direction.Bottom);
            ChunkDataGenerator.WalkingForTheBottomChunk(matriz, seed);
            InstantiateNewChunksInWorld(Direction.Top);
        }
    }

    void DestroyChunksInWorld(Direction direction)
    {
        
        if(direction == Direction.Left)
        {
            for(int row = 0; row < matriz.GetLength(0); row++)
            {
                Chunk chunk = matriz[row, 0];
                Destroy(chunk.GetChunkGameObject());
            }
        }
        else if(direction == Direction.Right)
        {
            for(int row = 0; row < matriz.GetLength(0); row++)
            {
                Chunk chunk = matriz[row, matriz.GetLength(1) - 1];
                Destroy(chunk.GetChunkGameObject());
            }
        }
        else if(direction == Direction.Top)
        {
            for(int col = 0; col < matriz.GetLength(1); col++)
            {
                Chunk chunk = matriz[0, col];
                Destroy(chunk.GetChunkGameObject());
            }
        }
        else if(direction == Direction.Bottom)
        {
            for(int col = 0; col < matriz.GetLength(1); col++)
            {
                Chunk chunk = matriz[matriz.GetLength(0) - 1, col];
                Destroy(chunk.GetChunkGameObject());
            }
        }
    }
    
    void InstantiateNewChunksInWorld(Direction direction)
    {
        if (direction == Direction.Left)
        {
            for (int row = 0; row < matriz.GetLength(0); row++)
                ChunkRender.Render(
                    matriz[row, 0],
                    wallPrefabScript,
                    cellSize,
                    chunkSize,
                    groundAndCeilingSize,
                    lamp
                );
        }
        else if (direction == Direction.Right)
        {
            int lastCol = matriz.GetLength(1) - 1;

            for (int row = 0; row < matriz.GetLength(0); row++)
                ChunkRender.Render(
                    matriz[row, lastCol],
                    wallPrefabScript,
                    cellSize,
                    chunkSize,
                    groundAndCeilingSize,
                    lamp
                );
        }
        else if (direction == Direction.Top)
        {
            for (int col = 0; col < matriz.GetLength(1); col++)
                ChunkRender.Render(
                    matriz[0, col],
                    wallPrefabScript,
                    cellSize,
                    chunkSize,
                    groundAndCeilingSize,
                    lamp
                );
        }
        else if (direction == Direction.Bottom)
        {
            int lastRow = matriz.GetLength(0) - 1;

            for (int col = 0; col < matriz.GetLength(1); col++)
                ChunkRender.Render(
                    matriz[lastRow, col],
                    wallPrefabScript,
                    cellSize,
                    chunkSize,
                    groundAndCeilingSize,
                    lamp
                );
        }
    }

    public Vector2Int GetRandomChunk()
    {
        int randomX = UnityEngine.Random.Range(0, matriz.GetLength(0) - 1);
        int randomY = UnityEngine.Random.Range(0, matriz.GetLength(1) - 1);

        return new Vector2Int(randomX, randomY);
    }

    public Chunk GetChunk(Vector2Int chunkPosition)
    {
        int row = chunkPosition.y - (playerChunk.y - (matriz.GetLength(0) / 2));
        int col = chunkPosition.x - (playerChunk.x - (matriz.GetLength(1) / 2));

        return matriz[row, col];
    }
    public void SetBlackout(bool isBlackout)
    {
        if(this.isBlackout != isBlackout)
        {
            this.isBlackout = isBlackout;

            for (int y = 0; y < matriz.GetLength(0); y++)
            {
                for (int x = 0; x < matriz.GetLength(1); x++)
                {
                    matriz[x,y].SetBlackout(this.isBlackout);
                }
            }
        }
    }
}
