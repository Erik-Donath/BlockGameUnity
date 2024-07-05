using System.Collections.Generic;

using ModelData;
using UnityEngine;

/*
 ChunkMesh:
 Hier wird das Unity GameObjekt des Chunks erstellt und das Mesh wird von den Block Daten generiert.
*/

public class ChunkMesh {
    private Vector2Int   _coord;
    private GameObject   _gameObject;
    private MeshRenderer _meshRenderer;
    private MeshFilter   _meshFilter;

    public ChunkMesh(Vector2Int coord) {
        _coord = coord;
        _gameObject = new GameObject();
        _gameObject.transform.position = new Vector3Int(coord.x, 0, coord.y) * Chunk.ChunkSize;
        _gameObject.transform.parent   = World.Instance.transform;
        _gameObject.name = $"Chunk {coord}";

        _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
        _meshFilter   = _gameObject.AddComponent<MeshFilter>();

        _meshRenderer.material = World.Material;
    }

    ~ChunkMesh() {
        Delete();
    }
    public void Delete() {
        _gameObject.transform.SetParent(null);
        _gameObject.name = "Deleted";
        _gameObject.SetActive(false);
        GameObject.DestroyImmediate(_gameObject, true);
    }

    public void BuildMesh(ref Chunk c) {
        // 1. Generiere alle Block Seiten f�r jeden Block x,y,z 
        List<MeshFace> faces = new List<MeshFace>();
        for(int y = 0; y < c.GetBlocks.GetLength(1); y++) {
            for(int x = 0; x < c.GetBlocks.GetLength(0); x++) {
                for(int z = 0; z < c.GetBlocks.GetLength(2); z++) {
                    // Bekomme Block Typ und position als Vector
                    byte block = c.GetBlocks[x, y, z];
                    ref Block blockType = ref Blocks.blocks[block];
                    Vector3Int pos = new Vector3Int(x, y, z);

                    // Ignoriere Luft oder invalide Bl�cke
                    if(block == 0 || block >= Blocks.blocks.Length)
                        continue;

                    // Lade Model und f�ge Block Seiten hinzu, wenn der Block bei dieser Seite nicht solide ist. Aka. Luft
                    Model model = blockType.Model;
                    for(int f = 0; f < 6; f++) {
                        Vector3Int posFace = pos + Data.InDirection[f];

                        if(!c.IsSolid(posFace)) {
                            faces.Add(new MeshFace(pos, (Direction)f, Data.GetTexture(model.Faces[f].Texture)));
                        }
                    }
                }
            }
        }

        // 2. Generiere Dreiecke
        Vector3[] positions = new Vector3[4 * faces.Count];
        Vector2[] uvs       = new Vector2[4 * faces.Count];
        int[]     triangles = new int[2 * 3 * faces.Count];

        // F�r jede Block Seite
        for(int i = 0; i < faces.Count; i++) {
            MeshFace face = faces[i];
            int vertexIndex   = i * 4;
            int triangleIndex = i * 6;

            // F�ge Block Seiten Ecken hinzu
            for(int j = 0; j < 4; j++) {
                int index = Data.Vertices[(int)face.Direction, j];
                positions[vertexIndex + j] = Data.Positions[index] + face.Position;
            }

            // Setzt die UV koordinaten
            Vector4 uv = face.UV;
            uvs[vertexIndex + 0] = new Vector2(uv.x, uv.y);
            uvs[vertexIndex + 1] = new Vector2(uv.x, uv.w);
            uvs[vertexIndex + 2] = new Vector2(uv.z, uv.y);
            uvs[vertexIndex + 3] = new Vector2(uv.z, uv.w);

            // F�ge die Dreiecke hinzu
            for(int j = 0; j < 6; j++) {
                int order = Data.Triangles[j];
                triangles[triangleIndex + j] = vertexIndex + order;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = $"Chunk Mesh {_coord}";

        // Aktualisiere Mesh Daten
        mesh.vertices  = positions;
        mesh.uv        = uvs;
        mesh.triangles = triangles;

        // Berechne Erneut die Normals, Bounds und Tangents.
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        _meshFilter.mesh = mesh;
    }
}
