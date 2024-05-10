using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField] private Material worldMaterial;

    private MeshFilter filter;

    VoxelMesh voxelMesh = new VoxelMesh();
    bool[,,] blocks = new bool[16, 16, 16];

    private void Start() {
        filter = this.AddComponent<MeshFilter>();
        MeshRenderer renderer = this.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = worldMaterial;

        for(int i = 0; i < blocks.GetLength(0); i++){
            for (int j = 0; j < blocks.GetLength(1); j++){
                for (int k = 0; k < blocks.GetLength(2); k++) {
                    blocks[i, j, k] = (i + j + k) % 2 == 0;
                }
            }
        }

        GenerateMesh();
    }

    private bool IsSolid(Vector3Int pos) {
        if (pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        ) return false;
        return blocks[pos.x, pos.y, pos.z];
    }

    private void GenerateMesh() {
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
            if (!IsSolid(position + VoxelData.inDirection[j])) {
                voxelMesh.AddFace(position, (FaceDirection)j, new Vector4(0, 0, 1, 1));
            }
        }
    }

}
