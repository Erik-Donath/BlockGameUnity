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

        chunk = new Chunk(new Vector2Int(0, 0));
        chunk.GameObject.transform.SetParent(transform, false);
        chunk.GenerateMesh();
    }

    private Chunk chunk;
    private static World instance = null;
}
