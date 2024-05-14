using System.Collections.Generic;

using BlockData;
using UnityEngine;

public class ChunkMesh {
    public const int ChunkSize = 16;

    private GameObject   _gameObject;
    private MeshRenderer _meshRenderer;
    private MeshFilter   _meshFilter;

    public ChunkMesh(ChunkCoord coord) {
        _gameObject = new GameObject();
        _gameObject.transform.position = coord.Position * ChunkSize;
        _gameObject.transform.parent   = World.Instance.transform;

        _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
        _meshFilter   = _gameObject.AddComponent<MeshFilter>();

        _meshRenderer.material = World.Instance.Material;
    }

    ~ChunkMesh() {
        Object.DestroyImmediate(_gameObject);
    }

    public void BuildMesh(ref byte[,,] blocks) {
        List<VoxelFace> faces = new List<VoxelFace>();
        for(int y = 0; y < ChunkSize; y++) {
            for(int x = 0; x < ChunkSize; x++) {
                for(int z = 0; z < ChunkSize; z++) {
                    byte block = blocks[x, y, z];
                    ref Block blockType = ref Block.Blocks[block];
                    Vector3Int pos = new Vector3Int(x, y, z);

                    if(block == 0 || block >= Block.Blocks.Length)
                        continue;

                    VoxelModel model = (VoxelModel)blockType.States.DefaultModel;
                    if(!model.IsPrimitv) continue;

                    int[] textures = model.Textures.Textures;


                    for(int f = 0; f < 6; f++) {
                        Vector3Int posFace = BlockData.Data.InDirection[f];

                        if(!IsSolid(ref blocks, posFace)) {
                            faces.Add(new BlockData.VoxelFace(pos, (FaceDirection)f, VoxelTextures.GetTexture(textures[f])));
                        }
                    }
                }
            }
        }

        Vector3[] positions = new Vector3[4 * faces.Count];
        Vector2[] uvs       = new Vector2[4 * faces.Count];
        int[] triangles     = new int[2 * 3 * faces.Count];

        for(int i = 0; i < faces.Count; i++) {
            VoxelFace face = faces[i];
            int vertexIndex = i * 4;
            int triangleIndex = i * 6;

            for(int j = 0; j < 4; j++) {
                int index = VoxelData.Indeces[(int)face.Direction, j];
                positions[vertexIndex + j] = VoxelData.Vertices[index] + face.Position;
            }

            Vector4 uv = face.UV;
            uvs[vertexIndex + 0] = new Vector2(uv.x, uv.y);
            uvs[vertexIndex + 1] = new Vector2(uv.x, uv.w);
            uvs[vertexIndex + 2] = new Vector2(uv.z, uv.y);
            uvs[vertexIndex + 3] = new Vector2(uv.z, uv.w);

            for(int j = 0; j < 6; j++) {
                int order = VoxelData.ArrayOrder[j];
                triangles[triangleIndex + j] = vertexIndex + order;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Chunk Mesh";

        mesh.vertices  = positions;
        mesh.uv        = uvs;
        mesh.triangles = triangles;

        //mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        //mesh.RecalculateTangents();

        _meshFilter.mesh = mesh;
    }

    private bool IsSolid(ref byte[,,] blocks, Vector3Int pos) {
        if( pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        ) return false;
        return Block.Blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }
}
