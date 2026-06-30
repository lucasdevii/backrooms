using UnityEngine;
using UnityEngine.UIElements;

public class Chunk
{
    private ulong worldSeed;
    private ulong chunkSeed;
    private Vector3Int position;

    public Chunk(ulong worldSeed, int chunkX, int chunkZ)
    {
        position.x = chunkX;
        position.z = chunkZ;

        this.worldSeed = worldSeed;

        chunkSeed = GenerateChunkSeed();
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
}