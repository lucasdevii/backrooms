using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    
    public ulong seed = 4196283291231231231;
    public Vector3Int playerChunk;
    public int chunkSize = 15;
    public int renderDistance = 12;
    public Chunk[,] matriz;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se o player n foi arrastado para o inspector, pega na cena
        if (playerPosition == null){
            playerPosition = GameObject.FindWithTag("Player").transform;
        }

        int matrizSize = (renderDistance * 2) + 1;
        matriz = new Chunk[matrizSize, matrizSize];
        
        DefinePlayerChunk();

        InstantiateMatrizOfChunks();
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
        playerChunk.y = Mathf.FloorToInt(playerPosition.position.y / chunkSize);
        playerChunk.z = Mathf.FloorToInt(playerPosition.position.z / chunkSize);
    }

    void InstantiateMatrizOfChunks()
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

}
