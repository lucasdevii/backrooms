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
    private int cellSize = 5;
    private int cellsQuantityInChunk = 32; 

    private int wallHeight = 6;
    private Vector3 groundAndCeilingSize;
  

    void Awake()
    {

        int matrizSize = (renderDistance * 2) + 1;

        chunkSize = cellSize * cellsQuantityInChunk;

        matriz = new Chunk[matrizSize, matrizSize];
        groundAndCeilingSize = new Vector3(chunkSize, 1, chunkSize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se o player n foi arrastado para o inspector, pega na cena
        if (playerPosition == null){
            playerPosition = GameObject.FindWithTag("Player").transform;
        }

        DefinePlayerChunk();

        FillinMatrizOfChunks();
        ConnectChunks();

        InstantiateChunksInWorld();
    }

    // Update is called once per frame
    void Update()
    {
        DefinePlayerChunk();
    }
    
    void DefinePlayerChunk()
    {
        if (playerPosition == null) return;

        Vector2Int currentChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerPosition.position.x / chunkSize), 
            Mathf.FloorToInt(playerPosition.position.z / chunkSize)
        );

        if(currentChunkPosition != playerChunk)
        {
            //A variação em x e y do chunk do player, para saber quais chunks carregar e quais chunks remover
            int dx = playerChunk.x - currentChunkPosition.x;
            int dy = playerChunk.y - currentChunkPosition.y;

            LoadNewChunks(currentChunkPosition, dx, dy);
        }

        playerChunk.x = currentChunkPosition.x; 
        playerChunk.y = currentChunkPosition.y;
    }

    void FillinMatrizOfChunks()
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
                matriz[row, col] = new Chunk(seed, currentChunkX, currentChunkY, cellsQuantityInChunk);

                currentChunkX++;
            }
            
            currentChunkY--;
        }   
    }

    //Conecta os chunks da matriz entre si, abrindo as paredes dos chunks a direita e abaixo de cada chunk
    void ConnectChunks()
    {
        for(int row = 0; row < matriz.GetLength(0); row++){
            for(int col = 0; col < matriz.GetLength(1); col++){
                Chunk currentChunk = matriz[row, col];

                //Conecta com o chunk da direita
                if(col + 1 < matriz.GetLength(1)){
                    Chunk rightChunk = matriz[row, col + 1];
                    currentChunk.ConnectWith(rightChunk, Direction.Right);
                }

                //Conecta com o chunk de baixo
                if(row + 1 < matriz.GetLength(0)){
                    Chunk bottomChunk = matriz[row + 1, col];
                    currentChunk.ConnectWith(bottomChunk, Direction.Bottom);
                }
            }
        }
    }

    //Instancia os gameObjects das paredes e chão.
    void InstantiateChunksInWorld()
    {


        Vector2Int currentPlayerChunkPosition = new Vector2Int(playerChunk.x * chunkSize, playerChunk.y * chunkSize);
        
        Vector2 initChunkPosition = new Vector2(
            currentPlayerChunkPosition.x - (chunkSize * renderDistance), 
            currentPlayerChunkPosition.y + (chunkSize * renderDistance)
        );

        //Começa no canto superior esquerdo da matriz, levando em consideração o canto superior da chunk
        Vector2 currentChunkOrigin = new Vector2(initChunkPosition.x, initChunkPosition.y);
        
        for(int chunkY = 0; chunkY < matriz.GetLength(0); chunkY++)
        {
            for(int chunkX = 0; chunkX < matriz.GetLength(1); chunkX++)
            {

                Chunk chunk = matriz[chunkY, chunkX];

                GameObject chunkObject = new GameObject($"Chunk {chunkX}, {chunkY}");

                chunk.SetChunkGameObject(chunkObject);

                //Cria o chão no tamanho da chunk, e o teto no mesmo tamanho, mas na altura da chunk
                GroundAndCeilingInstance(currentChunkOrigin, chunkObject);

                for(int cellY = 0; cellY < cellsQuantityInChunk; cellY++)
                {
                    for(int cellX = 0; cellX < cellsQuantityInChunk; cellX++)
                    {
                        //Pega a matriz de células do chunk atual                    
                        Vector2 cellPosition = new Vector2(
                            currentChunkOrigin.x + cellX * cellSize, 
                            currentChunkOrigin.y - cellY * cellSize
                        );

                        Cell cell = chunk.GetCell(cellX, cellY);

                        //Instancia paredes da célula
                        WallsInstance(cellPosition, cell, chunkObject);

                    }
                }

                currentChunkOrigin.x += chunkSize;
            }

            currentChunkOrigin.x = initChunkPosition.x;
            currentChunkOrigin.y -= chunkSize;
        }
    }

    void GroundAndCeilingInstance(
        Vector2 currentChunkOrigin, 
        GameObject chunkObject
    )
    {
        float midOfChunk = chunkSize / 2;

        //Instancia chão da chunk
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.position = new Vector3(
            currentChunkOrigin.x + midOfChunk, 
            0, 
            currentChunkOrigin.y - midOfChunk
        );
        ground.transform.localScale = new Vector3(
            groundAndCeilingSize.x, 
            groundAndCeilingSize.y, 
            groundAndCeilingSize.z
        );
        ground.transform.SetParent(chunkObject.transform);

        //Instancia teto da chunk
        GameObject ceiling = Instantiate(ground, chunkObject.transform);
        ceiling.transform.position = new Vector3(
            currentChunkOrigin.x + midOfChunk, 
            wallHeight, 
            currentChunkOrigin.y - midOfChunk
        );
        ceiling.transform.localScale = new Vector3(
            groundAndCeilingSize.x, 
            groundAndCeilingSize.y, 
            groundAndCeilingSize.z
        );
    }

    void WallsInstance(
        Vector2 cellPosition, 
        Cell cell,
        GameObject chunkObjectFather
    )
    {
        HashSet<Direction> openedWalls = cell.GetOpenedWalls();

        if(!openedWalls.Contains(Direction.Right))
        {
            //Deixa as paredes como filhas do gameObject do chunk, para melhor organização na hierarquia
            Wall wall = Instantiate(wallPrefabScript, chunkObjectFather.transform);

            wall.Inicialize(
                new Vector2(cellPosition.x + (cellSize / 2), cellPosition.y), 
                cellSize, 
                wallHeight, 
                90
            );
        }
        if(!openedWalls.Contains(Direction.Bottom))
        {
            Wall wall = Instantiate(wallPrefabScript, chunkObjectFather.transform);
            
            wall.Inicialize(
                new Vector2(cellPosition.x, cellPosition.y - (cellSize / 2)), 
                cellSize, 
                wallHeight, 
                0
            );

        }
    }
    void LoadNewChunks(Vector2Int newPlayerChunk, int dx, int dy)
    {
        if(dx > 0) //Player se moveu para a esquerda
        {
            DestroyChunksInWorld(Direction.Left);
            ChunkDataGenerator.WalkingForTheLeftChunk(matriz, seed, newPlayerChunk, renderDistance, cellsQuantityInChunk);
            InstantiateNewChunksInWorld(Direction.Left);
        }
        else if(dx < 0) //Player se moveu para a direita
        {
            DestroyChunksInWorld(Direction.Right);
            ChunkDataGenerator.WalkingForTheRightChunk(matriz, seed, newPlayerChunk, renderDistance, cellsQuantityInChunk);
            InstantiateNewChunksInWorld(Direction.Right);
        }
        if(dy > 0) //Player se moveu para baixo
        {
            DestroyChunksInWorld(Direction.Bottom);
            ChunkDataGenerator.WalkingForTheBottomChunk(matriz, seed, newPlayerChunk, renderDistance, cellsQuantityInChunk);
            InstantiateNewChunksInWorld(Direction.Bottom);
        }
        else if(dy < 0) //Player se moveu para cima
        {
            DestroyChunksInWorld(Direction.Top);
            ChunkDataGenerator.WalkingForTheTopChunk(matriz, seed, newPlayerChunk, renderDistance, cellsQuantityInChunk);
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
                RenderChunk(matriz[row, 0]);
        }
        else if (direction == Direction.Right)
        {
            int lastCol = matriz.GetLength(1) - 1;

            for (int row = 0; row < matriz.GetLength(0); row++)
                RenderChunk(matriz[row, lastCol]);
        }
        else if (direction == Direction.Top)
        {
            for (int col = 0; col < matriz.GetLength(1); col++)
                RenderChunk(matriz[0, col]);
        }
        else if (direction == Direction.Bottom)
        {
            int lastRow = matriz.GetLength(0) - 1;

            for (int col = 0; col < matriz.GetLength(1); col++)
                RenderChunk(matriz[lastRow, col]);
        }
    }

    void RenderChunk(Chunk chunk)
    {
        // Canto superior esquerdo do chunk
        Vector2 chunkOrigin = new Vector2(
            chunk.position.x * chunkSize,
            chunk.position.y * chunkSize + chunkSize
        );

        GameObject chunkObject = new GameObject($"Chunk {chunk.position.x}, {chunk.position.y}");

        GroundAndCeilingInstance(chunkOrigin, chunkObject);

        // Se adicionou um campo GameObject na classe Chunk
        chunk.SetChunkGameObject(chunkObject);

        for (int cellRow = 0; cellRow < cellsQuantityInChunk; cellRow++)
        {
            for (int cellCol = 0; cellCol < cellsQuantityInChunk; cellCol++)
            {
                Vector2 cellPosition = new Vector2(
                    chunkOrigin.x + cellCol * cellSize,
                    chunkOrigin.y - cellRow * cellSize
                );

                WallsInstance(
                    cellPosition,
                    chunk.GetCell(cellCol, cellRow),
                    chunkObject
                );
            }
        }
    }
}
