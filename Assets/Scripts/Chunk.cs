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

        GenerateTerain();
    }

    public void GenerateTerain() {
        for(int i = 0; i < blocks.GetLength(0); i++) {
            for(int j = 0; j < blocks.GetLength(1); j++) {
                for(int k = 0; k < blocks.GetLength(2); k++) {
                    Vector3Int realPos = new Vector3Int(i, j, k) + Coord.Position * ChunkSize;
                    byte block;

                    switch(realPos.y) {
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
                    //block = 4;
                    blocks[i, j, k] = block;
                }
            }
        }
    }

    public void GenerateMesh() {
        Chunk c = this;
        mesh.BuildMesh(ref c);
    }

    public bool InChunkLocal(Vector3Int pos) {
        return (
            pos.x < 0 || pos.x >= ChunkSize ||
            pos.y < 0 || pos.y >= ChunkSize ||
            pos.z < 0 || pos.z >= ChunkSize
        );
    }

    public bool IsSolidLocal(Vector3Int pos) {
        if(InChunkLocal(pos)) {
            return false;
        }
        return Blocks.blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }

    private Vector3Int ConvertToWorldPos(Vector3Int pos) { // Consider only chunks that are +x +y +z
        Vector3Int cpos = Coord.Position;
        if(cpos.x < 0 || cpos.y < 0 || cpos.z < 0) {
            return Vector3Int.zero;
        }
        return pos + cpos * ChunkSize; 
    }
}
