using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField, Range(0, 10_000)] private int worldBorder = 1000;
    [SerializeField, Range(1, 100)] private int renderDistance = 5;
    [SerializeField] private Material material;
    [SerializeField] private int HorizontalTextureSize = 16; // x

    public static int Seed {
        get; set;
    }
    
    public int GetHorizontalTextureSize() {
        return HorizontalTextureSize;
    }
    public int WorldBorder {
        get => worldBorder;
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
    public Vector3Int PlayerBlockCoord {
        get {
            Vector3 pos = player.transform.position;
            return new Vector3Int((int)MathF.Floor(pos.x), (int)MathF.Floor(pos.y), (int)MathF.Floor(pos.z));
        }
    }

    public void AddUpdateChunk(Vector2Int c) {
        updateChunks.Add(new Vector2Int(c.x + 0, c.y + 0));
        updateChunks.Add(new Vector2Int(c.x - 1, c.y + 0));
        updateChunks.Add(new Vector2Int(c.x + 0, c.y - 1));
        updateChunks.Add(new Vector2Int(c.x + 1, c.y + 0));
        updateChunks.Add(new Vector2Int(c.x + 0, c.y + 1));
    }

    public bool SetBlock(Vector3Int block, Blocks.BlockId id) {
        int cs = Chunk.ChunkSize;

        if(block.x < 0 || block.y < 0 || block.z < 0)
            return false;
        if(block.x >= cs * worldBorder || block.y >= cs || block.z >= cs * worldBorder)
            return false;

        Vector3Int local = new Vector3Int(block.x % cs, block.y % cs, block.z % cs);
        Vector2Int chunk = new Vector2Int(block.x / cs, block.z / cs);

        if(block.y / cs != 0)
            return false;
        if(chunks.TryGetValue(chunk, out var c)) {
            bool res = c.SetBlock(local, id);
            if(res) AddUpdateChunk(chunk);
            return res;
        }
        return false;
    }

    public bool IsSolidLocal(Vector3Int block, Vector2Int fromChunk) {
        int cs = Chunk.ChunkSize;
        return IsSolid(new Vector3Int(fromChunk.x * cs, 0, fromChunk.y * cs) + block);
    }
    public bool IsSolid(Vector3Int block) {
        int cs = Chunk.ChunkSize;

        if(block.x < 0 || block.y < 0 || block.z < 0) return false;
        if(block.x >= cs * worldBorder || block.y >= cs || block.z >= cs * worldBorder) return false;

        Vector3Int local = new Vector3Int(block.x % cs, block.y % cs, block.z % cs);
        Vector2Int chunk = new Vector2Int(block.x / cs, block.z / cs);
        if(block.y / cs != 0) return false;
        if(chunks.TryGetValue(chunk, out var c)) {
            return c.IsSolid(local, true);
        }
        return true;
    }

    private void Start() {
        instance = this;
        Blocks.Init();

        UnityEngine.Random.InitState(Seed);
        Debug.Log("Current Seed is: " + Seed);

        int cs = Chunk.ChunkSize;
        int border = World.Instance.WorldBorder;
        player.transform.position = new Vector3(border * cs / 2, cs + 1, border * cs / 2);
        Debug.Log(player.transform.position);

        chunks = new();
        createChunks = new();
        deleteChunks = new();
        updateChunks = new();

        int px = PlayerChunkCoord.x;
        int py = PlayerChunkCoord.y;
        for(int x = px - 5; x < px + 5; x++) {
            for(int y = py - 5; y < py + 5; y++) {
                Vector2Int coord = new Vector2Int(x, y);
                Chunk c = new Chunk(coord);
                c.GenerateTerain();
                createChunks.Add(coord);
                chunks[coord] = c;
            }
        }
    }
    private void Update() {
        int px = PlayerChunkCoord.x;
        int py = PlayerChunkCoord.y;
        for(int x = px - renderDistance; x < px + renderDistance; x++) {
            for(int y = py - renderDistance; y < py + renderDistance; y++) {
                if(x < 0 || x >= worldBorder)
                    continue;
                if(y < 0 || y >= worldBorder)
                    continue;

                Vector2Int coord = new Vector2Int(x, y);
                if(!chunks.ContainsKey(coord))
                    createChunks.Add(coord);
            }
        }

        foreach(var pp in chunks.Keys) {
            if(pp.x < px - renderDistance || pp.x >= px + renderDistance ||
               pp.y < py - renderDistance || pp.y >= py + renderDistance
            ) {
                createChunks.Remove(pp);
                updateChunks.Remove(pp);
                deleteChunks.Add(pp);
            }
        }
        foreach(var pp in createChunks) {
            if(pp.x < px - renderDistance || pp.x >= px + renderDistance ||
               pp.y < py - renderDistance || pp.y >= py + renderDistance
            ) deleteChunks.Add(pp);
        }

        createChunks.ExceptWith(deleteChunks);
        updateChunks.ExceptWith(deleteChunks);

        HashSet<Vector2Int> deleteDeletes = new HashSet<Vector2Int>();
        foreach(var pp in deleteChunks) {
            if(!chunks.ContainsKey(pp)) deleteDeletes.Add(pp);
        }
        deleteDeletes.ExceptWith(deleteDeletes);

        if(!createChunksTask && createChunks.Count > 0)
            StartCoroutine(CreateChunks());

        if(!updateChunksTask && updateChunks.Count > 0)
            StartCoroutine(UpdateChunks());

        if(!deleteChunksTask && deleteChunks.Count > 0)
            StartCoroutine(DeleteChunks());
    }

    private IEnumerator CreateChunks() {
        Debug.Log("Started Creating Chunks");
        createChunksTask = true;
        while(createChunks.Count > 0) {
            Vector2Int coord = createChunks.First();
            if(!chunks.ContainsKey(coord)) {
                Chunk c = new Chunk(coord);
                c.GenerateTerain();
                chunks[coord] = c;
                AddUpdateChunk(coord);
            }
            createChunks.Remove(coord);
            yield return null;
        }
        createChunksTask = false;
        Debug.Log("Stoped Creating Chunks");
    }
    private IEnumerator UpdateChunks() {
        Debug.Log("Started Updating Chunks");
        updateChunksTask = true;
        while(updateChunks.Count > 0) {
            Vector2Int coord = updateChunks.First();
            if(chunks.TryGetValue(coord, out var chunk)) {
                chunk.UpdateMesh();
            }
            updateChunks.Remove(coord);
            yield return null;
        }
        updateChunksTask = false;
        Debug.Log("Stoped Updating Chunks");
    }
    private IEnumerator DeleteChunks() {
        Debug.Log("Started Deleting Chunks");
        deleteChunksTask = true;
        while(deleteChunks.Count > 0) {
            Vector2Int coord = deleteChunks.First();
            if(chunks.TryGetValue(coord, out var chunk)) {
                chunk.Delete();
                chunks.Remove(coord);
                GC.Collect();
            }
            deleteChunks.Remove(coord);
            createChunks.Remove(coord);
            updateChunks.Remove(coord);
            yield return null;
        }
        deleteChunksTask = false;
        Debug.Log("Stoped Deleting Chunks");
    }

    private static World instance = null;

    private Dictionary<Vector2Int, Chunk> chunks;
    private HashSet<Vector2Int> createChunks;
    private HashSet<Vector2Int> deleteChunks;
    private HashSet<Vector2Int> updateChunks;
    private bool createChunksTask = false;
    private bool deleteChunksTask = false;
    private bool updateChunksTask = false;
}
