using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Positions {
    public static Vector3Int FromArrayPositionToChunkPosition(Vector3Int block, Vector3Int chunk) {
        int cs = Chunk.ChunkSize;

        if(block.x < 0)   chunk.x -= 1;
        if(block.x >= cs) chunk.x += 1;
        if(block.y < 0)   chunk.y -= 1;
        if(block.y >= cs) chunk.y += 1;
        if(block.z < 0)   chunk.z -= 1;
        if(block.z >= cs) chunk.z += 1;

        return new Vector3Int(
            (chunk.x >= 0) ? block.x : cs - block.x - 1,
            (chunk.y >= 0) ? block.y : cs - block.y - 1,
            (chunk.z >= 0) ? block.z : cs - block.z - 1
        );
    }
    public static Vector3Int FromChunkPositionToWorldPosition(Vector3Int block, Vector3Int chunk) {
        return block + chunk * Chunk.ChunkSize;
    }
    public static Vector3Int FromArrayPositionToWorldPosition(Vector3Int block, Vector3Int chunk) {
        return FromChunkPositionToWorldPosition(FromArrayPositionToChunkPosition(block, chunk), chunk);
    }
}
