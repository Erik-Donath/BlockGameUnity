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
        block.x = (pos.x < 0) ? (cs + pos.x % cs) : (pos.x % cs);
        block.y = (pos.y < 0) ? (cs + pos.y % cs) : (pos.y % cs);
        block.z = (pos.z < 0) ? (cs + pos.z % cs) : (pos.z % cs);

        Vector3Int chunkCoord = pos;
        chunkCoord.x = (pos.x < 0) ? (pos.x / cs - 1) : pos.x / cs;
        chunkCoord.y = (pos.y < 0) ? (pos.y / cs - 1) : pos.y / cs;
        chunkCoord.z = (pos.z < 0) ? (pos.z / cs - 1) : pos.z / cs;
        //Debug.Log($"Pos-> {pos}, Block -> {block}, Chunk -> {chunkCoord}");

        if(Chunks.TryGetValue(new ChunkCoord(chunkCoord), out Chunk chunk)) {
            return chunk.IsSolid(block);
        }
        return false;
    }

    private void Start() {
        instance = this;
        Blocks.Init();

        for(int x = -2; x <= 2; x++) {
            for(int y = -2; y <= 2; y++) {
                ChunkCoord coord = new ChunkCoord(x, 0, y);
                Debug.Log($"{coord.Position}");
                CreateChunk(coord);
                Debug.Log("p");
            }
        }
    }

    private void CreateChunk(ChunkCoord coord) {
        Chunk chunk = new Chunk(coord);
        chunk.GenerateMesh();
        Chunks[coord] = chunk;
    }

    private static World instance = null;
}
