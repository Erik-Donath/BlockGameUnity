using System.Collections.Generic;

using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] private Material  material;
    [SerializeField] private Vector4[] textures;

    public static World Instance {
        get => instance;
    }
    public Material Material {
        get => material;
    }

    private void Start() {
        instance = this;
        CreateChunk(new Vector3Int(0, 0, 0));
    }

    private void CreateChunk(Vector3Int coord) {
        Chunk chunk = new Chunk(coord);
        chunk.GameObject.transform.SetParent(transform, false);
        chunk.GenerateMesh();
        chunks.Add(chunk);
    }

    private List<Chunk> chunks = new List<Chunk>();
    private static World instance = null;
}
