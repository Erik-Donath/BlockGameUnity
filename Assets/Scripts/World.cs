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
    public Material Material {
        get => material;
    }

    private void Start() {
        instance = this;
        for(int x = -10; x <= 10; x++) {
            for(int y = -10; y <= 10; y++) {
                CreateChunk(new ChunkCoord(x, 0, y));
            }
        }
    }

    private void CreateChunk(ChunkCoord coord) {
        Chunk chunk = new Chunk(coord);
        chunk.GenerateMesh();
        chunks.Add(chunk);
    }

    private List<Chunk> chunks = new List<Chunk>();
    private static World instance = null;
}
