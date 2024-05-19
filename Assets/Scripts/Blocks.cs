using System;

public static class Blocks {
    public enum BlockId: byte {
        Air   = 0,
        Stone = 1,
        Dirt  = 2,
        Grass = 3,
    };

    public static Model[] models {
        get; private set;
    }

    public static Block[] blocks {
        get; private set;
    }

    public static void Init() {
        int blockCount = Enum.GetNames(typeof(BlockId)).Length;
        models = new Model[blockCount];
        blocks = new Block[blockCount];

        models[(int)BlockId.Air]   = new Model(new ModelFace(0));
        models[(int)BlockId.Stone] = new Model(new ModelFace(4));
        models[(int)BlockId.Dirt]  = new Model(new ModelFace(1));
        models[(int)BlockId.Grass] = new Model(new ModelFace(3), new ModelFace(1), new ModelFace(2));

        blocks[(int)BlockId.Air]   = new Block("Air",   false, models[(int)BlockId.Air]);
        blocks[(int)BlockId.Stone] = new Block("Stone", true,  models[(int)BlockId.Stone]);
        blocks[(int)BlockId.Dirt]  = new Block("Dirt",  true,  models[(int)BlockId.Dirt]);
        blocks[(int)BlockId.Grass] = new Block("Grass", true,  models[(int)BlockId.Grass]);
    }
}
