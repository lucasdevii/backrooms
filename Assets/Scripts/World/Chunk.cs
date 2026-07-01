using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk
{
    private ulong worldSeed;
    private ulong chunkSeed;
    private int cellsQuantity = 32;
    private Vector3Int position;
    private Cell[,] internalGrid;
    

    public Chunk(ulong worldSeed, int chunkX, int chunkZ)
    {
        position.x = chunkX;
        position.z = chunkZ;

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
        chunkSeed ^= (ulong)position.z * 0xC2B2AE3D27D4EB4FUL;

        chunkSeed ^= chunkSeed >> 30;
        chunkSeed *= 0xBF58476D1CE4E5B9UL;
        chunkSeed ^= chunkSeed >> 27;
        chunkSeed *= 0x94D049BB133111EBUL;
        chunkSeed ^= chunkSeed >> 31;

        return chunkSeed;
    }
    
    void GenerateCellsInGrid()
    {
        Noise.DefaultNoise(chunkSeed, position.x, position.y);
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