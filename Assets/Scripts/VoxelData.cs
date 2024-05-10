using System.Collections.Generic;
using UnityEngine;

public static class VoxelData {
    // X+ => East
    // X- => West
    // Z+ => North
    // Z- => South
    // Y+ => Up
    // Y- => Down

    public static readonly Vector3[] Vertices = new Vector3[8] {
        new Vector3(0, 0, 0), // 0
        new Vector3(1, 0, 0), // 1
        new Vector3(0, 1, 0), // 2
        new Vector3(1, 1, 0), // 3
        new Vector3(0, 0, 1), // 4
        new Vector3(1, 0, 1), // 5
        new Vector3(0, 1, 1), // 6
        new Vector3(1, 1, 1), // 7
    };

    public static readonly int[,] Indeces = new int[6, 4] {
        { 5, 7, 4, 6 }, // North
        { 0, 2, 1, 3 }, // South

        { 4, 6, 0, 2 }, // West
        { 1, 3, 5, 7 }, // East

        { 2, 6, 3, 7 }, // Up
        { 1, 5, 0, 4 }, // Down
    };

    public static readonly int[] ArrayOrder = new int[6] {
        0, 1, 2, 2, 1, 3
    };

    public static readonly Vector3Int[] InDirection = new Vector3Int[6] {
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

public readonly struct VoxelFace {
    public readonly Vector3Int Position;
    public readonly FaceDirection Direction;
    public readonly Vector4 UV;

    public VoxelFace(Vector3Int pos, FaceDirection direction, Vector4 uv) {
        this.Position = pos;
        this.Direction = direction;
        this.UV = uv;
    }
}

public class VoxelMesh {
    private List<VoxelFace> faces = new List<VoxelFace>();
    public void ClearMesh()
    {
        faces.Clear();
    }

    public void AddFace(VoxelFace face) {
        faces.Add(face);
    }
    public void AddFace(Vector3Int pos, FaceDirection direction, Vector4 uv) {
        faces.Add(new VoxelFace(pos, direction, uv));
    }

    public void SetFaces(List<VoxelFace> faces) {
        this.faces = faces;
    }
    public List<VoxelFace> GetFaces() {
        return faces;
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
                int index = VoxelData.Indeces[(int)face.Direction, j];
                vertices[vertexIndex + j] = VoxelData.Vertices[index] + face.Position;
            }

            Vector4 uv = face.UV;
            uvs[vertexIndex + 0] = new Vector2(uv.x,      uv.y);
            uvs[vertexIndex + 1] = new Vector2(uv.x,      uv.y+uv.w);
            uvs[vertexIndex + 2] = new Vector2(uv.x+uv.z, uv.y);
            uvs[vertexIndex + 3] = new Vector2(uv.x+uv.z, uv.y+uv.w);
            
            for(int j = 0; j < 6; j++) {
                int order = VoxelData.ArrayOrder[j];
                indeces[triangleIndex + j] = vertexIndex + order;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Chunk Mesh";

        mesh.vertices  = vertices;
        mesh.uv        = uvs;
        mesh.triangles = indeces;

        //mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        //mesh.RecalculateTangents();

        return mesh;
    }
}