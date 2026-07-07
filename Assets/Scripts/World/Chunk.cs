using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk
{
    // Removido o 'private' e adicionado 'public' com 'protected set'
    public ulong worldSeed { get; protected set; }
    public ulong chunkSeed { get; protected set; } 
    public int cellsQuantity { get; private set; } = 32; // Também transformada em propriedade de leitura
    public Vector2Int position { get; protected set; }
    public Cell[,] internalGrid; 

    public Chunk(ulong worldSeed, int chunkX, int chunkZ)
    {
        position = new Vector2Int(chunkX, chunkZ);

        this.worldSeed = worldSeed;
        internalGrid = new Cell[cellsQuantity, cellsQuantity];

        chunkSeed = GenerateChunkSeed();

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