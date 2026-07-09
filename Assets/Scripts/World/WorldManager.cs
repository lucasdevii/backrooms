using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;

    public ulong seed = 4196283291231231231;
    public Vector2Int playerChunk = new Vector2Int();
    public int renderDistance = 12;
    public Chunk[,] matriz;

    //Cerca de 4 unidades de distancia em game
    private int cellSize = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se o player n foi arrastado para o inspector, pega na cena
        if (playerPosition == null){
            playerPosition = GameObject.FindWithTag("Player").transform;
        }

        int matrizSize = (renderDistance * 2) + 1;
        matriz = new Chunk[matrizSize, matrizSize];
        
        cellSize = matriz[0,0].GetLength(0);

        DefinePlayerChunk();

        FillinMatrizOfChunks();
        ConnectChunks();

        
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
        int initialChunkY = playerChunk.z + matrizCenter;

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
        for(int chunkY = 0; chunkY < matriz.GetLength(0); chunkY++)
        {
            for(int chunkX = 0; chunkX < matriz.GetLength(1); chunkX++)
            {
                //Instancia chão e teto da chunk
                //...

                for(int cellY = 0; cellY < matriz[chunkY, chunkX].GetLength(0); cellY++)
                {
                    for(int cellX = 0; cellX < matriz[chunkY, chunkX].GetLength(1); cellX++)
                    {
                        //Instancia paredes da célula
                        //...
                        
                    }
                }
            }
        }
    }

}
