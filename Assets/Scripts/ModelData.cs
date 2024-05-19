using UnityEngine;

// X+ => East
// X- => West
// Z+ => North
// Z- => South
// Y+ => Up
// Y- => Down

namespace ModelData {
    public enum Direction : int {
        North = 0,
        South = 1,
        West = 2,
        East = 3,
        Up = 4,
        Down = 5,
    }

    public readonly struct MeshFace {
        public readonly Vector3Int Position;
        public readonly Direction Direction;
        public readonly Vector4 UV;

        public MeshFace(Vector3Int pos, Direction direction, Vector4 uv) {
            Position = pos;
            Direction = direction;
            UV = uv;
        }
    }

    public static class Data {
        public static Vector4 GetTexture(int textureID) {
            World world     = World.Instance;
            Texture texture = World.Material.mainTexture;

            int x = textureID % world.GetHorizontalTextureCount();
            int y = textureID / world.GetHorizontalTextureCount();

            float width = texture.width / world.GetHorizontalTextureCount();
            float height = texture.height / world.GetVerticalTextureCount();

            Vector4 vec = new Vector4(
                (width * x) / texture.width,
                (height * y) / texture.height,
                (width * (x + 1.0f)) / texture.width,
                (height * (y + 1.0f)) / texture.height
            );
            return vec;
        }

        public static readonly Vector3Int[] InDirection = new Vector3Int[6] {
            new Vector3Int(0, 0, +1), // North
            new Vector3Int(0, 0, -1), // South

            new Vector3Int(-1, 0, 0), // West
            new Vector3Int(+1, 0, 0), // East

            new Vector3Int(0, +1, 0), // Up
            new Vector3Int(0, -1, 0), // Down
        };

        public static readonly Vector3[] Positions = new Vector3[8] {
            new Vector3(0, 0, 0), // 0
            new Vector3(1, 0, 0), // 1
            new Vector3(0, 1, 0), // 2
            new Vector3(1, 1, 0), // 3
            new Vector3(0, 0, 1), // 4
            new Vector3(1, 0, 1), // 5
            new Vector3(0, 1, 1), // 6
            new Vector3(1, 1, 1), // 7
        };

        public static readonly int[,] Vertices = new int[6, 4] {
            { 5, 7, 4, 6 }, // North
            { 0, 2, 1, 3 }, // South

            { 4, 6, 0, 2 }, // West
            { 1, 3, 5, 7 }, // East

            { 2, 6, 3, 7 }, // Up
            { 1, 5, 0, 4 }, // Down
        };

        public static readonly int[] Triangles = new int[6] {
            0, 1, 2, 2, 1, 3
        };
    }
}
