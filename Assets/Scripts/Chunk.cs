using System;
using System.Drawing;

using UnityEngine;

public class Chunk {
    public const int ChunkSize = 16;

    public Vector2Int Coord {
        get; private set;
    }
    public byte[,,] GetBlocks {
        get => blocks;
    }

    private ChunkMesh mesh;
    private byte[,,] blocks;

    public Chunk(Vector2Int coord) {
        Coord = coord;

        mesh = new ChunkMesh(coord);
        blocks = new byte[ChunkSize, ChunkSize, ChunkSize];
        Debug.Log($"Created Chunk {Coord}");
    }
    ~Chunk() {
        mesh = null;
        blocks = null;
        Delete();
    }

    public void Delete() {
        mesh.Delete();
        Debug.Log($"Deleted Chunk {Coord}");
    }


    public void GenerateTerain() {
        for(int x = 0; x < blocks.GetLength(0); x++) {
            for(int z = 0; z < blocks.GetLength(2); z++) {
                int height = (int)Mathf.Floor(Noise.Get2DPerlinNoise(new Vector2(x, z) + Coord * ChunkSize, 31415.0f, 0.075218249f) * (blocks.GetLength(1) - 4 - 2) + 4);
                if(height > blocks.GetLength(1)) height = blocks.GetLength(1) - 1;
                for(int y = 0; y < blocks.GetLength(1); y++) {
                    float pers = ((float)y / height) * 100.0f;
                    Blocks.BlockId id = Blocks.BlockId.Air;
                    switch(pers) {
                        case >= 0 and < 70:
                            id = Blocks.BlockId.Stone;
                            break;
                        case >= 70 and < 100:
                            id = Blocks.BlockId.Dirt;
                            break;
                    }

                    if(y == height)
                        id = Blocks.BlockId.Grass;
                    if(y > height)
                        id = Blocks.BlockId.Air;
                    if(y == 0)
                        id = Blocks.BlockId.Bedrock;
                    blocks[x, y, z] = (byte)id;
                }
            }
        }
        World.Instance.AddUpdateChunk(Coord);
    }

    public void UpdateMesh() {
        Chunk c = this;
        mesh.BuildMesh(ref c);
    }

    public bool NotInChunkLocal(Vector3Int pos) {
        return (
            pos.x < 0 || pos.x >= ChunkSize ||
            pos.y < 0 || pos.y >= ChunkSize ||
            pos.z < 0 || pos.z >= ChunkSize
        );
    }

    public bool IsSolid(Vector3Int pos, bool noWorld = false) { // noWorld ist um zu verhinder das es zum Loop kommt. Auch wenn das unmöglich sein sollte!
        if(NotInChunkLocal(pos)) {
            if(noWorld) {
                Debug.Log($"Failed to get solid on {pos} in {Coord}.");
                return false;
            }
            return World.Instance.IsSolidLocal(pos, Coord);
        }
        return Blocks.blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }

    public bool SetBlock(Vector3Int pos, Blocks.BlockId id) {
        if(NotInChunkLocal(pos)) {
            Debug.Log($"Failed to set block on {pos} in {Coord}.");
            return false;
        }
        blocks[pos.x, pos.y, pos.z] = (byte)id;
        return true;
    }
}

/*
 Chunk:
    Diese Klasse beschreibt die Strukture und funktionalität eines Chunks.
*/