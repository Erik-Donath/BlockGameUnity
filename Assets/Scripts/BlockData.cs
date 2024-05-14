using UnityEngine;

namespace BlockData {
    public struct BlockNabours {
        public bool North, South, West, East, Up, Down;

        public bool[] Solids {
            get => new bool[6] { North, South, West, East, Up, Down };
            set {
                North = value[0];
                South = value[1];
                West  = value[2];
                East  = value[3];
                Up    = value[4];
                Down  = value[5];
            }
        }
        public BlockNabours(bool[] solids) {
            North = solids[0];
            South = solids[1];
            West  = solids[2];
            East  = solids[3];
            Up    = solids[4];
            Down  = solids[5];
        }
    }

    public enum FaceDirection : int {
        North = 0,
        South = 1,
        West = 2,
        East = 3,
        Up = 4,
        Down = 5,
    }

    public readonly struct VoxelFace {
        public readonly Vector3Int Position;
        public readonly FaceDirection Direction;
        public readonly Vector4 UV;

        public VoxelFace(Vector3Int pos, FaceDirection direction, Vector4 uv) {
            Position = pos;
            Direction = direction;
            UV = uv;
        }
    }

    public struct VoxelTextures {
        public int North, South, West, East, Up, Down;

        public int[] Textures {
            get => new int[6] { North, South, West, East, Up, Down };
            set {
                North = value[0];
                South = value[1];
                West = value[2];
                East = value[3];
                Up = value[4];
                Down = value[5];
            }
        }
        public VoxelTextures(int[] textures) {
            North = textures[0];
            South = textures[1];
            West = textures[2];
            East = textures[3];
            Up = textures[4];
            Down = textures[5];
        }
        public VoxelTextures(int texture) {
            North = texture;
            South = texture;
            West = texture;
            East = texture;
            Up = texture;
            Down = texture;
        }
        public VoxelTextures(int up, int down, int side) {
            North = side;
            South = side;
            West = side;
            East = side;
            Up = up;
            Down = down;
        }

        public static Vector4 GetTexture(int textureID) {
            World world = World.Instance;
            Texture texture = world.Material.mainTexture;

            int x = textureID % world.GetHorizontalTextureCount();
            int y = textureID / world.GetHorizontalTextureCount();

            float width = texture.width / world.GetHorizontalTextureCount();
            float height = texture.height / world.GetVerticalTextureCount();

            Vector4 vec = new Vector4(
                (width * x) / texture.width,
                (height * y) / texture.height,
                (width * (x + 1)) / texture.width,
                (height * (y + 1)) / texture.height
            );
            return vec;
        }
    }

    public static class Data {
        public static readonly Vector3Int[] InDirection = new Vector3Int[6] {
            new Vector3Int(0, 0, +1), // North
            new Vector3Int(0, 0, -1), // South

            new Vector3Int(-1, 0, 0), // West
            new Vector3Int(+1, 0, 0), // East

            new Vector3Int(0, +1, 0), // Up
            new Vector3Int(0, -1, 0), // Down
        };
    }
}
