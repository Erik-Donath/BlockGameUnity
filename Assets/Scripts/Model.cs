using System.Collections.Generic;
using UnityEngine;

public struct RenderDirections {
    public bool North, South, West, East, Up, Down;

    public bool[] Get {
        get => new bool[6] { North, South, West, East, Up, Down };
    }
}

public interface Model {
    public List<VoxelFace> GetFaces(Vector3Int pos, RenderDirections dirs);
}

public struct PrimitivTextures {
    public readonly int north, south, west, east, up, down;

    public int[] Get {
        get => new int[6] {
            north, south, west, east, up, down
        };
    }
    public Vector4[] GetUVs {
        get => new Vector4[6] {
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 1, 1),
            new Vector4(0, 0, 1, 1),
        };
    }

    public PrimitivTextures(int texture) {
        north = south = west = east = up = down = texture;
    }
    public PrimitivTextures(int upDown, int sides) {
        north = south = west = east = sides;
        up = down = upDown;
    }
    public PrimitivTextures(int up, int down, int sides) {
        north = south = west = east = sides;
        this.up = up;
        this.down = down;
    }

    public PrimitivTextures(int north, int south, int west, int east, int up, int down) {
        this.north = north;
        this.south = south;
        this.west = west;
        this.east = east;
        this.up = up;
        this.down = down;
    }
}

public class PrimitivModel : Model {
    public readonly PrimitivTextures PrimitivTextures;

    public PrimitivModel(PrimitivTextures primitivTextures) {
        PrimitivTextures = primitivTextures;
    }

    public List<VoxelFace> GetFaces(Vector3Int pos, RenderDirections dirs) {
        List<VoxelFace> faces = new List<VoxelFace>();

        bool[] directions = dirs.Get;
        Vector4[] uvs = PrimitivTextures.GetUVs;

        for(int i = 0; i < 6; i++) {
            if(directions[i])
                faces.Add(new VoxelFace(pos, (FaceDirection)i, uvs[i]));
        }

        return faces;
    }
}
