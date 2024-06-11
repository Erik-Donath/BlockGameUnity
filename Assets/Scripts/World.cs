using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField, Range(0, 10_000)] private int worldBorder = 1000;
    [SerializeField, Range(1, 20)] private int renderDistance = 5;
    [SerializeField] private int seed = 0;
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

    public Vector2Int PlayerChunkCoord {
        get {
            Vector3 pos = player.transform.position;
            return new Vector2Int((int)pos.x / Chunk.ChunkSize, (int)pos.z / Chunk.ChunkSize);
        }
    }
    public void AddUpdateChunk(Vector2Int c) {
        updateChunks.Add(new Vector2Int(c.x - 1, c.y + 0));
        updateChunks.Add(new Vector2Int(c.x + 0, c.y - 1));
        updateChunks.Add(new Vector2Int(c.x + 0, c.y + 0));
        updateChunks.Add(new Vector2Int(c.x + 1, c.y + 0));
        updateChunks.Add(new Vector2Int(c.x + 0, c.y + 1));
    }


    private void Start() {
        instance = this;
        Blocks.Init();

        UnityEngine.Random.InitState(seed);

        int cs = Chunk.ChunkSize;
        chunks = new Chunk[worldBorder, worldBorder];
        activeChunks = new List<Vector2Int>();
        deleteChunks = new List<Vector2Int>();
        updateChunks = new List<Vector2Int>();
        player.transform.position = new Vector3(chunks.GetLength(0) * cs / 2, cs + 1, chunks.GetLength(1) * cs / 2);

    }
    private void Update() {
        int px = PlayerChunkCoord.x;
        int py = PlayerChunkCoord.y;
        for(int x = px - renderDistance; x < px + renderDistance; x++) {
            for(int y = py - renderDistance; y < py + renderDistance; y++) {
                if(x < 0 || x >= worldBorder) continue;
                if(y < 0 || y >= worldBorder) continue;

                if(chunks[x, y] == null) {
                    Vector2Int v = new Vector2Int(x, y);
                    Chunk c = new Chunk(v);
                    c.GenerateTerain();
                    activeChunks.Add(v);
                    chunks[x, y] = c;
                }
            }
        }
        List<Vector2Int> nowActive = activeChunks.Distinct().ToList();
        List<Vector2Int> nowUpdate = updateChunks.Distinct().ToList();
        List<Vector2Int> nowDelete = deleteChunks.Distinct().ToList();
        deleteChunks.Clear();
        updateChunks.Clear();

        foreach(Vector2Int c in nowActive) {
            if(c.x < px - renderDistance || c.x >= px + renderDistance ||
               c.y < py - renderDistance || c.y >= py + renderDistance
            ) deleteChunks.Add(c);
        }

        foreach(Vector2Int c in nowUpdate) {
            chunks[c.x, c.y]?.UpdateMesh();
        }

        foreach(Vector2Int c in nowDelete) {
            AddUpdateChunk(c); // Update Nachtbar chunks
            activeChunks.Remove(c);
            chunks[c.x, c.y]?.Delete();
            chunks[c.x, c.y] = null;
            GC.Collect();
        }
    }

    private static World instance = null;
    private Chunk[,] chunks;
    private List<Vector2Int> activeChunks;
    private List<Vector2Int> deleteChunks;
    private List<Vector2Int> updateChunks;

    public bool IsSolidLocal(Vector3Int block, Vector2Int fromChunk) {
        int cs = Chunk.ChunkSize;
        return IsSolid(new Vector3Int(fromChunk.x * cs, 0, fromChunk.y * cs) + block);
    }

    public bool IsSolid(Vector3Int block) {
        int cs = Chunk.ChunkSize;

        if(block.x < 0 || block.y < 0 || block.z < 0) return false;
        if(block.x >= cs * chunks.GetLength(0) || block.y >= cs || block.z >= cs * chunks.GetLength(1)) return false;

        Vector3Int local = new Vector3Int(block.x % cs, block.y % cs, block.z % cs);
        Vector3Int chunk = new Vector3Int(block.x / cs, block.y / cs, block.z / cs);
        if(chunk.y != 0) return false;
        if (local.x < 0 || local.y < 0 || local.z < 0) {
            Debug.Log("oh");
            return false;
        }
        if (chunks[chunk.x, chunk.z] == null) return false;
        return chunks[chunk.x, chunk.z].IsSolid(local, true);
    }
}
