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
    public byte[,,] GetBlocks {
        get => blocks;
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
                    //float c = (Mathf.Sin(2 * Mathf.PI * (i / 16.0f) + (j / 16.0f) + (k / 16.0f)) + 1) / 2;
                    //block = (byte)(c * 4);

                    //Debug.Log(block);
                    //block = (byte)((i + j + k) % Block.Blocks.Length);
                    block = 4;
                    blocks[i, j, k] = block;
                }
            }
        }
    }

    public void GenerateMesh() {
        Chunk c = this;
        mesh.BuildMesh(ref c);
    }

    public bool IsSolid(Vector3Int pos) {
        if( pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        ) {
            return false;
            //Vector3Int worldPos = Positions.FromArrayPositionToWorldPosition(pos, Coord.Position);
            //return World.IsSolid(worldPos);
        }
        return Blocks.blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }
}
