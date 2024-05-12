using BlockData;

using UnityEngine;
using UnityEngine.UIElements;

public class Chunk {
    public const int ChunkSize = 16;
    public GameObject GameObject {
        get => gameObject;
    }
    public Vector3Int Coord {
        get => new Vector3Int((int)gameObject.transform.position.x / ChunkSize, (int)gameObject.transform.position.y / ChunkSize, (int)gameObject.transform.position.z / ChunkSize);
        set => gameObject.transform.position = value * ChunkSize;
    }

    public Chunk(Vector3Int pos) {
        gameObject = new GameObject();
        gameObject.name = "Chunk " + pos.x + ", " + pos.y + ", " + pos.z;
        Coord = pos;

        filter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = World.Instance.Material;

        for(int i = 0; i < blocks.GetLength(0); i++) {
            for(int j = 0; j < blocks.GetLength(1); j++) {
                for(int k = 0; k < blocks.GetLength(2); k++) {
                    blocks[i, j, k] = (byte)((i + j + k) % Block.Blocks.Length);
                }
            }
        }
    }

    ~Chunk() {
        gameObject.SetActive(false);
        Object.Destroy(gameObject);
    }

    public void GenerateMesh() {
        voxelMesh.ClearMesh();

        for (int i = 0; i < blocks.GetLength(0); i++) {
            for (int j = 0; j < blocks.GetLength(1); j++) {
                for (int k = 0; k < blocks.GetLength(2); k++) {
                    byte block = blocks[i, j, k];

                    if(block != 0 && block < Block.Blocks.Length) {
                        AddBlockFaces(new Vector3Int(i, j, k), block);
                    }
                }
            }
        }

        filter.mesh = voxelMesh.GenerateMesh();
    }

    private void AddBlockFaces(Vector3Int position, byte block) {
        IModel model = Block.Blocks[block].States.DefaultModel;

        if(model.IsPrimitv) {
            VoxelModel vmodel = (VoxelModel)model;

            BlockNabours nabours = GetNaboursDetails(position);
            voxelMesh.AddFaces(vmodel.GetFaces(position, nabours));
        }
        else throw new System.NotImplementedException("Nur primitve(voxel) blöcke sind unterstützt.");
    }

    private BlockData.BlockNabours GetNaboursDetails(Vector3Int pos) {
        if( pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        ) return new BlockData.BlockNabours();

        bool[] solids = new bool[6];
        for(int i = 0; i < 6; i++)
            solids[i] = IsSolid(pos + VoxelData.InDirection[i]);
        return new BlockData.BlockNabours(solids);
    }

    private bool IsSolid(Vector3Int pos) {
        if(pos.x < 0 || pos.x >= blocks.GetLength(0) ||
            pos.y < 0 || pos.y >= blocks.GetLength(1) ||
            pos.z < 0 || pos.z >= blocks.GetLength(2)
        )
            return false;
        return Block.Blocks[blocks[pos.x, pos.y, pos.z]].Solid;
    }

    private GameObject gameObject;
    private MeshFilter filter;

    private VoxelMesh voxelMesh = new VoxelMesh();
    private byte[,,] blocks = new byte[ChunkSize, ChunkSize, ChunkSize];
}
