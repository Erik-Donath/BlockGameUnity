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
