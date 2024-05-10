using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField] private Material worldMaterial;

    private MeshFilter m_filter;
    private MeshRenderer m_renderer;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> indeces = new List<int>();
    private int triangleIndex = 0;

    private void Start() {
        m_filter = this.AddComponent<MeshFilter>();
        m_renderer = this.AddComponent<MeshRenderer>();
        m_renderer.sharedMaterial = worldMaterial;

        GenerateMesh();
    }

    private void GenerateMesh() {
        ClearMesh();

        for (int a = 0; a < 5; a++) {
            for (int b = 0; b < 5; b++) {
                for (int c = 0; c < 5; c++) {
                    AddVoxel(new Vector3(a, b, c));
                }
            }
        }
        SendMesh();
    }

    private void AddVoxel(Vector3 position) {
        for(int j = 0; j < 6; j++) {
            for(int i = 0; i < 6; i++) {
                int index = VoxelData.voxelIndeces[j, i];
                vertices.Add(position + VoxelData.voxelVertices[index]);
                uvs.Add(VoxelData.voxelUvs[i]);

                indeces.Add(i + triangleIndex);
            }
            triangleIndex += 6;
        }
    }

    private void ClearMesh() {
        vertices.Clear();
        uvs.Clear();
        indeces.Clear();
        triangleIndex = 0;
    }


    private void SendMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indeces.ToArray();
        mesh.uv = uvs.ToArray();
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        mesh.name = "Chunk Mesh - " + transform.position.x + ", " + transform.position.y;
        m_filter.mesh = mesh;
    }
}
