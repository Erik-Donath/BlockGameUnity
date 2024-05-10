using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VoxelData {
    // X+ => East
    // X- => West
    // Z+ => North
    // Z- => South
    // Y+ => Up
    // Y- => Down

    public static readonly Vector3[] vertices = new Vector3[8] {
        new Vector3(0, 0, 0), // 0
        new Vector3(1, 0, 0), // 1
        new Vector3(0, 1, 0), // 2
        new Vector3(1, 1, 0), // 3
        new Vector3(0, 0, 1), // 4
        new Vector3(1, 0, 1), // 5
        new Vector3(0, 1, 1), // 6
        new Vector3(1, 1, 1), // 7
    };

    public static readonly int[,] indeces = new int[6, 4] {
        { 5, 7, 4, 6 }, // North
        { 0, 2, 1, 3 }, // South

        { 4, 6, 0, 2 }, // West
        { 1, 3, 5, 7 }, // East

        { 2, 6, 3, 7 }, // Up
        { 1, 5, 0, 4 }, // Down
    };

    public static readonly int[] arrayOrder = new int[6] {
        0, 1, 2, 2, 1, 3
    };

    public static readonly Vector3Int[] inDirection = new Vector3Int[6] {
        new Vector3Int(0, 0, +1), // North
        new Vector3Int(0, 0, -1), // South

        new Vector3Int(-1, 0, 0), // West
        new Vector3Int(+1, 0, 0), // East

        new Vector3Int(0, +1, 0), // Up
        new Vector3Int(0, -1, 0), // Down
    };
}

public enum FaceDirection: int {
    North = 0,
    South = 1,
    West  = 2,
    East  = 3,
    Up    = 4,
    Down  = 5,
}

public struct VoxelFace {
    public readonly Vector3Int pos;
    public readonly FaceDirection dir;
    public readonly Vector4 uv;

    public VoxelFace(Vector3Int pos, FaceDirection direction, Vector4 uv) {
        this.pos = pos;
        this.dir = direction;
        this.uv = uv;
    }
}

public class VoxelMesh {
    private List<VoxelFace> faces = new List<VoxelFace>();

    public void AddFace(VoxelFace face) {
        faces.Add(face);
    }
    public void AddFace(Vector3Int pos, FaceDirection direction, Vector4 uv) {
        faces.Add(new VoxelFace(pos, direction, uv));
    }

    public void ClearMesh() {
        faces.Clear();
    }

    public Mesh GenerateMesh() {
        Vector3[] vertices = new Vector3[faces.Count * 4];
        Vector2[] uvs = new Vector2[faces.Count * 4];
        int[] indeces = new int[faces.Count * 6];

        for(int i = 0; i < faces.Count; i++) {
            VoxelFace face = faces[i];
            int vertexIndex = i * 4;
            int triangleIndex = i * 6;

            for(int j = 0; j < 4; j++) {
                int index = VoxelData.indeces[(int)face.dir, j];
                vertices[vertexIndex + j] = VoxelData.vertices[index] + face.pos;
            }

            Vector4 uv = face.uv;
            uvs[vertexIndex + 0] = new Vector2(uv.x,      uv.y);
            uvs[vertexIndex + 1] = new Vector2(uv.x,      uv.y+uv.w);
            uvs[vertexIndex + 2] = new Vector2(uv.x+uv.z, uv.y);
            uvs[vertexIndex + 3] = new Vector2(uv.x+uv.z, uv.y+uv.w);
            
            for(int j = 0; j < 6; j++) {
                int order = VoxelData.arrayOrder[j];
                indeces[triangleIndex + j] = vertexIndex + order;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Chunk Mesh";

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indeces.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }
}