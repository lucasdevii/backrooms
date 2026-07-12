using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
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
  

    void Awake()
    {
        int matrizSize = (renderDistance * 2) + 1;
        
        chunkSize = cellSize * cellsQuantityInChunk;
        matriz = new Chunk[matrizSize, matrizSize];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Console.WriteLine("Seed: " + seed);

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

        playerChunk.x = Mathf.FloorToInt(playerPosition.position.x / chunkSize); 
        playerChunk.y = Mathf.FloorToInt(playerPosition.position.z / chunkSize);
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
                    currentChunk.ConnectWith(rightChunk, Cell.Direction.Right);
                }

                //Conecta com o chunk de baixo
                if(row + 1 < matriz.GetLength(0)){
                    Chunk bottomChunk = matriz[row + 1, col];
                    currentChunk.ConnectWith(bottomChunk, Cell.Direction.Bottom);
                }
            }
        }
    }

    //Instancia os gameObjects das paredes e chão.
    void InstantiateChunksInWorld()
    {
        int wallHeight = 6;

        Vector2Int currentPlayerChunkPosition = new Vector2Int(playerChunk.x * chunkSize, playerChunk.y * chunkSize);
        
        Vector2 initChunkPosition = new Vector2(
            currentPlayerChunkPosition.x - (chunkSize * renderDistance), 
            currentPlayerChunkPosition.y + (chunkSize * renderDistance)
        );

        //Começa no canto superior esquerdo da matriz, levando em consideração o canto superior da chunk
        Vector2 currentChunkOrigin = new Vector2(initChunkPosition.x, initChunkPosition.y);

        //Tamanho geral do chão e teto da chunk, que é o mesmo tamanho da chunk, mas com altura 1
        Vector3 groundAndCeilingSize = new Vector3(chunkSize, 1, chunkSize);
        
        for(int chunkY = 0; chunkY < matriz.GetLength(0); chunkY++)
        {
            for(int chunkX = 0; chunkX < matriz.GetLength(1); chunkX++)
            {
                //Cria o chão no tamanho da chunk, e o teto no mesmo tamanho, mas na altura da chunk
                GroundAndCeilingInstance(currentChunkOrigin, groundAndCeilingSize, wallHeight);

                Chunk chunk = matriz[chunkY, chunkX];

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
                        WallsInstance(cellPosition, cell, wallHeight);

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
        Vector3 groundAndCeilingSize, 
        int wallHeight
    )
    {
        float midOfChunk = chunkSize / 2;

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.position = new Vector3(
            currentChunkOrigin.x + midOfChunk, 
            0, 
            currentChunkOrigin.y - midOfChunk
        );
        //Escala o chão e teto da chunk para o tamanho da chunk, mas com altura 1
        ground.transform.localScale = new Vector3(
            groundAndCeilingSize.x, 
            groundAndCeilingSize.y, 
            groundAndCeilingSize.z
        );

        //Instancia chão e teto da chunk
        GameObject ceiling = Instantiate(ground);
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
        int wallHeight
    )
    {
        HashSet<Cell.Direction> openedWalls = cell.GetOpenedWalls();

        if(!openedWalls.Contains(Cell.Direction.Right))
        {
            Wall wall = Instantiate(wallPrefabScript);
            wall.Inicialize(
                new Vector2(cellPosition.x + (cellSize / 2), cellPosition.y), 
                cellSize, 
                wallHeight, 
                90
            );
        }
        if(!openedWalls.Contains(Cell.Direction.Bottom))
        {
            Wall wall = Instantiate(wallPrefabScript);
            wall.Inicialize(
                new Vector2(cellPosition.x, cellPosition.y - (cellSize / 2)), 
                cellSize, 
                wallHeight, 
                0
            );

        }
    }

}
