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
    public ChunkCoord(Vector3Int position) {
        X = position.x;
        Y = position.y;
        Z = position.z;
    }

    public override string ToString() {
        return $"{X}, {Y}, {Z}";
    }
}

public class Chunk {
    public const int ChunkSize = 16;

    public ChunkCoord Coord {
        get; private set;
    }

    private ChunkMesh mesh;
    private byte[,,] blocks;

    public Chunk(ChunkCoord coord) {
        Coord = coord;

        mesh = new ChunkMesh(coord);
        blocks = new byte[ChunkSize, ChunkSize, ChunkSize];

        for(int i = 0; i < blocks.GetLength(0); i++) {
            for(int j = 0; j < blocks.GetLength(1); j++) {
                for(int k = 0; k < blocks.GetLength(2); k++) {
                    byte block;

                    switch(j) {
                        case >= 0 and < 10:
                            block = (byte)Blocks.BlockId.Stone;
                            break;
                        case >= 10 and < 15:
                            block = (byte)Blocks.BlockId.Dirt;
                            break;
                        case 15:
                            block = (byte)Blocks.BlockId.Grass;
                            break;
                        default:
                            block = (byte)Blocks.BlockId.Air;
                            break;
                    }

                    //block = (byte)((i + j + k) % Block.Blocks.Length);
                    blocks[i, j, k] = block;
                }
            }
        }
    }

    public void GenerateMesh() {
        mesh.BuildMesh(ref blocks);
    }

    public bool IsSolid(Vector3Int pos) {
        if(pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        ) {
            return World.IsSolid(Coord.Position * ChunkSize + pos);
        }
        return Blocks.blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }
}
