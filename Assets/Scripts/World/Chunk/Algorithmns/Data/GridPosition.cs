// using System.Collections.Generic;
// using UnityEngine;

// public class GridPosition
// {
//     public Chunk Chunk { get; private set; }
//     public Cell Cell { get; private set; }

//     public Vector2Int ChunkIndex { get; private set; }
//     public Vector2Int CellIndex { get; private set; }
//     public Vector2 ChunkOrigin { get; private set; }
    
//     private float chunkSize = WorldManager.Instance.chunkSize;

//     public void Update(Vector3 worldPosition)
//     {
//         // Atualiza chunk e célula
//         Vector2Int newChunkIndex = new Vector2Int(
//             Mathf.FloorToInt(
//                 worldPosition.x / chunkSize
//             ),
//             Mathf.FloorToInt(
//                 worldPosition.z / chunkSize
//             )
//         );

//         if (ChunkIndex != newChunkIndex)
//         {
//             ChunkIndex = newChunkIndex;
//             Chunk = WorldManager.Instance.GetChunk(ChunkIndex);
//             ChunkOrigin = new Vector2(newChunkIndex.x * chunkSize, newChunkIndex.y * chunkSize);
//         }

//         Vector2Int newCellIndex = new Vector2Int(
//             Mathf.FloorToInt((worldPosition.x - ChunkOrigin.x) / WorldManager.cellSize), 
//             Mathf.FloorToInt((worldPosition.z - ChunkOrigin.y) / WorldManager.cellSize)
//         );

//         if(CellIndex != newCellIndex)
//         {
//             Cell = Chunk.GetCell(newCellIndex.x, newCellIndex.y);
//             CellIndex = newCellIndex;
//         }
//     }
// }