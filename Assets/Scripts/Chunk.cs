using UnityEngine;

public class Chunk {
    public const int ChunkSize = 16;
    public GameObject GameObject {
        get => gameObject;
    }

    public Chunk(Vector2Int pos) {
        gameObject = new GameObject();
        gameObject.transform.position = new Vector3Int(pos.x, 0, pos.y) * ChunkSize;
        gameObject.name = "Chunk " + pos.x + ", " + pos.y;
        
        filter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = World.Instance.Material;

        for(int i = 0; i < blocks.GetLength(0); i++) {
            for(int j = 0; j < blocks.GetLength(1); j++) {
                for(int k = 0; k < blocks.GetLength(2); k++) {
                    blocks[i, j, k] = (i + j + k) % 2 == 0;
                }
            }
        }
    }

    ~Chunk() {
        Object.Destroy(gameObject, 0.0f);
    }

    public void GenerateMesh() {
        voxelMesh.ClearMesh();

        for (int i = 0; i < blocks.GetLength(0); i++) {
            for (int j = 0; j < blocks.GetLength(1); j++) {
                for (int k = 0; k < blocks.GetLength(2); k++) {
                    if (IsSolid(new Vector3Int(i, j, k))) {
                        AddVoxel(new Vector3Int(i, j, k));
                    }
                }
            }
        }

        filter.mesh = voxelMesh.GenerateMesh();
    }

    private void AddVoxel(Vector3Int position) {
        for(int j = 0; j < 6; j++) {
            if (!IsSolid(position + VoxelData.InDirection[j])) {
                voxelMesh.AddFace(position, (FaceDirection)j, new Vector4(0, 0, 1, 1));
            }
        }
    }

    private bool IsSolid(Vector3Int pos) {
        if(pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        )
            return false;
        return blocks[pos.x, pos.y, pos.z];
    }

    private GameObject gameObject;
    private MeshFilter filter;

    private VoxelMesh voxelMesh = new VoxelMesh();
    private bool[,,] blocks = new bool[16, 16, 16];
}
