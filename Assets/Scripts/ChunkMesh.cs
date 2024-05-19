using System.Collections.Generic;
using System.Drawing;

using ModelData;
using UnityEngine;

public class ChunkMesh {
    private ChunkCoord   _coord;
    private GameObject   _gameObject;
    private MeshRenderer _meshRenderer;
    private MeshFilter   _meshFilter;

    public ChunkMesh(ChunkCoord coord) {
        _coord = coord;
        _gameObject = new GameObject();
        _gameObject.transform.position = coord.Position * Chunk.ChunkSize;
        _gameObject.transform.parent   = World.Instance.transform;
        _gameObject.name = $"Chunk {coord}";

        _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
        _meshFilter   = _gameObject.AddComponent<MeshFilter>();

        _meshRenderer.material = World.Material;
    }

    ~ChunkMesh() {
        Object.DestroyImmediate(_gameObject);
    }

    public void BuildMesh(ref byte[,,] blocks) {
        List<MeshFace> faces = new List<MeshFace>();
        for(int y = 0; y < blocks.GetLength(1); y++) {
            for(int x = 0; x < blocks.GetLength(0); x++) {
                for(int z = 0; z < blocks.GetLength(2); z++) {
                    byte block = blocks[x, y, z];
                    ref Block blockType = ref Blocks.blocks[block];
                    Vector3Int pos = new Vector3Int(x, y, z);

                    if(block == 0 || block >= Blocks.blocks.Length)
                        continue;

                    Model model = blockType.Model;
                    for(int f = 0; f < 6; f++) {
                        Vector3Int posFace = pos + Data.InDirection[f];

                        if(!IsSolid(ref blocks, posFace)) {
                            faces.Add(new MeshFace(pos, (Direction)f, Data.GetTexture(model.Faces[f].Texture)));
                        }
                    }
                }
            }
        }

        Vector3[] positions = new Vector3[4 * faces.Count];
        Vector2[] uvs       = new Vector2[4 * faces.Count];
        int[]     triangles = new int[2 * 3 * faces.Count];

        for(int i = 0; i < faces.Count; i++) {
            MeshFace face = faces[i];
            int vertexIndex   = i * 4;
            int triangleIndex = i * 6;

            for(int j = 0; j < 4; j++) {
                int index = Data.Vertices[(int)face.Direction, j];
                positions[vertexIndex + j] = Data.Positions[index] + face.Position;
            }

            Vector4 uv = face.UV;
            uvs[vertexIndex + 0] = new Vector2(uv.x, uv.y);
            uvs[vertexIndex + 1] = new Vector2(uv.x, uv.w);
            uvs[vertexIndex + 2] = new Vector2(uv.z, uv.y);
            uvs[vertexIndex + 3] = new Vector2(uv.z, uv.w);

            for(int j = 0; j < 6; j++) {
                int order = Data.Triangles[j];
                triangles[triangleIndex + j] = vertexIndex + order;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Chunk Mesh";

        mesh.vertices  = positions;
        mesh.uv        = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        _meshFilter.mesh = mesh;
    }

    private bool IsSolid(ref byte[,,] blocks, Vector3Int pos) {
        if( pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        ) {
            return World.IsSolid(_coord.Position * Chunk.ChunkSize + pos);
        }
        return Blocks.blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }
}
