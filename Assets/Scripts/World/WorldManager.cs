using System;
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
    public ulong seed = 4196283291231231231;
    public Vector2Int playerChunk = new Vector2Int();
    public int renderDistance = 1;
    public Chunk[,] matriz;

    //--------------- CHUNK ----------------
    private int chunkSize; //Tamanho de cada chunk em unidades de escala do game

    //-------------- CÉLULAs ---------------
    public static int cellSize = 5;
    public static int cellsQuantityInChunk = 32; 

    public static float wallHeight = 6.5f;
    private Vector3 groundAndCeilingSize; //Precisa ter o mesmo tamanho que o chunkSize
  

    void Awake()
    {

        int matrizSize = (renderDistance * 2) + 1;

        chunkSize = cellSize * cellsQuantityInChunk;

        matriz = new Chunk[matrizSize, matrizSize];
        groundAndCeilingSize = new Vector3(chunkSize, 0.5f, chunkSize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se o player n foi arrastado para o inspector, pega na cena
        if (playerPosition == null){
            playerPosition = GameObject.FindWithTag("Player").transform;
        }

        DefinePlayerChunk();

        FillInMatrizOfChunks();
        ChunkRender.ConnectChunks(matriz);

        InstantiateChunksInWorld();
    }

    // Update is called once per frame
    void Update()
    {
        VerifyIfChunkChange();
    }

    void DefinePlayerChunk()
    {
        playerChunk.x = Mathf.FloorToInt(playerPosition.position.x / chunkSize);
        playerChunk.y = Mathf.FloorToInt(playerPosition.position.z / chunkSize);
    }
    
    void VerifyIfChunkChange()
    {
        
        if (playerPosition == null) return;

        Vector2Int currentChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerPosition.position.x / chunkSize), 
            Mathf.FloorToInt(playerPosition.position.z / chunkSize)
        );

        if(currentChunkPosition != playerChunk)
        {
            Debug.Log($"Mudou de chunk | current chunk: {currentChunkPosition.x}, {currentChunkPosition.y} | ");
            //A variação em x e y do chunk do player, para saber quais chunks carregar e quais chunks remover
            int dx = playerChunk.x - currentChunkPosition.x;
            int dy = playerChunk.y - currentChunkPosition.y;

            LoadNewChunks(currentChunkPosition, dx, dy);
        }

        playerChunk.x = currentChunkPosition.x; 
        playerChunk.y = currentChunkPosition.y;

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
                ChunkRender.Render(
                    matriz[row, col],
                    wallPrefabScript,
                    cellSize,
                    chunkSize,
                    groundAndCeilingSize
                );
            }
        }
    }

    void LoadNewChunks(Vector2Int newPlayerChunk, int dx, int dy)
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
                Debug.Log($"Renderizando chunk {chunk.position}");
                Destroy(chunk.GetChunkGameObject());
            }
        }
        else if(direction == Direction.Right)
        {
            for(int row = 0; row < matriz.GetLength(0); row++)
            {
                Chunk chunk = matriz[row, matriz.GetLength(1) - 1];
                Debug.Log($"Renderizando chunk {chunk.position}");
                Destroy(chunk.GetChunkGameObject());
            }
        }
        else if(direction == Direction.Top)
        {
            for(int col = 0; col < matriz.GetLength(1); col++)
            {
                Chunk chunk = matriz[0, col];
                Debug.Log($"Renderizando chunk {chunk.position}");
                Destroy(chunk.GetChunkGameObject());
            }
        }
        else if(direction == Direction.Bottom)
        {
            for(int col = 0; col < matriz.GetLength(1); col++)
            {
                Chunk chunk = matriz[matriz.GetLength(0) - 1, col];
                Debug.Log($"Renderizando chunk {chunk.position}");
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
                    groundAndCeilingSize
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
                    groundAndCeilingSize
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
                    groundAndCeilingSize
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
                    groundAndCeilingSize
                );
        }
    }
}
