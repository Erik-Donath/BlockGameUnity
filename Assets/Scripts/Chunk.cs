using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private readonly Vector3[] voxelVertices = new Vector3[8] {
        new Vector3(0.0f, 0.0f, 0.0f), // 0
        new Vector3(1.0f, 0.0f, 0.0f), // 1
        new Vector3(1.0f, 1.0f, 0.0f), // 2
        new Vector3(0.0f, 1.0f, 0.0f), // 3
        new Vector3(0.0f, 0.0f, 1.0f), // 4
        new Vector3(1.0f, 0.0f, 1.0f), // 5
        new Vector3(1.0f, 1.0f, 1.0f), // 6
        new Vector3(0.0f, 1.0f, 1.0f), // 7
    };

    private readonly int[,] voxelIndeces = new int[6, 6] {
        { 0, 3, 1, 1, 3, 2 }, // Back
        { 5, 6, 4, 4, 6, 7 }, // Front
        { 3, 7, 2, 2, 7, 6 }, // Top
        { 1, 5, 0, 0, 5, 4 }, // Bottom
        { 4, 7, 0, 0, 7, 3 }, // Left
        { 1, 2, 5, 5, 2, 6 }, // Right
    };

    private readonly Vector2[] voxelUvs = new Vector2[6] {
        new Vector2(0.0f, 0.0f), // 0
        new Vector2(0.0f, 1.0f), // 1
        new Vector2(1.0f, 0.0f), // 2
        new Vector2(1.0f, 0.0f), // 3
        new Vector2(0.0f, 1.0f), // 4
        new Vector2(1.0f, 1.0f), // 5
    };

    [SerializeField] private Material worldMaterial;


    private MeshFilter m_filter;
    private MeshRenderer m_renderer;

    private void Start() {
        m_filter = this.AddComponent<MeshFilter>();
        m_renderer = this.AddComponent<MeshRenderer>();
        m_renderer.sharedMaterial = worldMaterial;

        CreateMesh();
    }

    private void Update() {
        
    }

    private void CreateMesh() {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> indeces = new List<int>();

        // Create Mesh
        for(int j = 0; j < 6; j++) {
            for(int i = 0; i < 6; i++) {
                int index = voxelIndeces[j, i];
                vertices.Add(voxelVertices[index]);
                uvs.Add(voxelUvs[i]);
                indeces.Add(j * 6 + i);
            }
        }


        // Send Mesh to Unity
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indeces.ToArray();
        mesh.uv = uvs.ToArray();
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        m_filter.mesh = mesh;
    }
}
