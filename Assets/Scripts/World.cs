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
        Vector3Int coord = pos / Chunk.ChunkSize;
        Vector3Int block = new Vector3Int(pos.x % Chunk.ChunkSize, pos.y % Chunk.ChunkSize, pos.z % Chunk.ChunkSize);

        Debug.Log($"{pos}: {coord} <> {block}");

        if(Chunks.TryGetValue(new ChunkCoord(coord), out Chunk chunk)) {
            return chunk.IsSolid(block);
        }
        return false;
    }

    private void Start() {
        instance = this;
        Blocks.Init();

        for(int x = 0; x <= 0; x++) {
            for(int y = 0; y <= 0; y++) {
                ChunkCoord coord = new ChunkCoord(x, 0, y);
                CreateChunk(coord);
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
