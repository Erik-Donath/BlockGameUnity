using System.Collections.Generic;

using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] private Material material;
    [SerializeField] private int HorizontalTextureCount = 1; // x
    [SerializeField] private int VerticalTextureCount   = 1; // y

    public int GetHorizontalTextureCount() {
        return HorizontalTextureCount;
    }
    public int GetVerticalTextureCount() {
        return VerticalTextureCount;
    }

    public static World Instance {
        get => instance;
    }
    public static Material Material {
        get => Instance.material;
    }

    public static Dictionary<ChunkCoord, Chunk> Chunks {
        get; private set;
    } = new Dictionary<ChunkCoord, Chunk>();

    public static bool IsSolid(Vector3Int pos) {
        int cs = Chunk.ChunkSize;

        Vector3Int block = pos;
        block.x = (block.x >= 0) ? (block.x % cs) : (cs + block.x % cs - 1);
        block.y = (block.y >= 0) ? (block.y % cs) : (cs + block.y % cs - 1);
        block.z = (block.z >= 0) ? (block.z % cs) : (cs + block.z % cs - 1);

        Vector3Int chunk = pos;
        chunk.x = (chunk.x >= 0) ? (chunk.x / cs) : (chunk.x / cs - 1);
        chunk.y = (chunk.y >= 0) ? (chunk.y / cs) : (chunk.y / cs - 1);
        chunk.z = (chunk.z >= 0) ? (chunk.z / cs) : (chunk.z / cs - 1);

        if(Chunks.TryGetValue(new ChunkCoord(chunk), out Chunk c)) {
            //Debug.Log($"{pos} - {chunk}: {block}");
            return c.IsSolid(block);
        }
        return false;
    }

    private void Start() {
        instance = this;
        Blocks.Init();

        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                for(int z = -1; z <= 1; z++) {
                    ChunkCoord coord = new ChunkCoord(x, y, z);
                    CreateChunk(coord);
                }
            }
        }
        foreach(var c in Chunks) {
            c.Value.GenerateMesh();
        }
    }

    private void CreateChunk(ChunkCoord coord) {
        Chunk chunk = new Chunk(coord);
        Chunks[coord] = chunk;
    }

    private static World instance = null;
}
