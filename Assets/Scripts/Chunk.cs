using BlockData;

using UnityEngine;

public struct ChunkCoord {
    int X, Y, Z;

    public Vector3Int Position {
        set {
            X = value.x;
            Y = value.y;
            Z = value.z;
        }
        get => new Vector3Int(X, Y, Z);
    }

    public ChunkCoord(int x, int y, int z) {
        X = x;
        Y = y;
        Z = z;
    }
}

public class Chunk {
    public ChunkCoord Coord {
        get; private set;
    }

    private ChunkMesh mesh;
    private byte[,,] blocks;

    public Chunk(ChunkCoord coord) {
        Coord = coord;

        mesh = new ChunkMesh(coord);
        blocks = new byte[ChunkMesh.ChunkSize, ChunkMesh.ChunkSize, ChunkMesh.ChunkSize];

        for(int i = 0; i < blocks.GetLength(0); i++) {
            for(int j = 0; j < blocks.GetLength(1); j++) {
                for(int k = 0; k < blocks.GetLength(2); k++) {
                    blocks[i, j, k] = (byte)((i + j + k) % Block.Blocks.Length);
                    if(j == 15)
                        blocks[i, j, k] = 2;
                    else
                        blocks[i, j, k] = 1;
                }
            }
        }
    }

    public void GenerateMesh() {
        mesh.BuildMesh(ref blocks);
    }

    private bool IsSolid(Vector3Int pos) {
        if(pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        )
            return false;
        return Block.Blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }
}
