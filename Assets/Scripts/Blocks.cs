using System;

public static class Blocks {
    public enum BlockId: byte {
        Air     = 0,
        Stone   = 1,
        Dirt    = 2,
        Grass   = 3,
        Bedrock = 4,
        Bricks  = 5,
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

        models[(int)BlockId.Air]     = new Model(new ModelFace(0));
        models[(int)BlockId.Stone]   = new Model(new ModelFace(4));
        models[(int)BlockId.Dirt]    = new Model(new ModelFace(1));
        models[(int)BlockId.Grass]   = new Model(new ModelFace(3), new ModelFace(1), new ModelFace(2));
        models[(int)BlockId.Bedrock] = new Model(new ModelFace(5));
        models[(int)BlockId.Bricks]  = new Model(new ModelFace(6));

        blocks[(int)BlockId.Air]     = new Block("Air", false, models[(int)BlockId.Air]);
        blocks[(int)BlockId.Stone]   = new Block("Stone", true, models[(int)BlockId.Stone]);
        blocks[(int)BlockId.Dirt]    = new Block("Dirt", true, models[(int)BlockId.Dirt]);
        blocks[(int)BlockId.Grass]   = new Block("Grass", true, models[(int)BlockId.Grass]);
        blocks[(int)BlockId.Bedrock] = new Block("Bedrock", true, models[(int)BlockId.Bedrock]);
        blocks[(int)BlockId.Bricks]  = new Block("Bricks", true, models[(int)BlockId.Bricks]);
    }
}

/*
    Blocks:
    Enthält ein enum welches die Block sorte als byte angibt. Enthält alle Models sowie Blöcke im Spiel.
    Blöcke:
        Air, Stone, Dirt, Grass, Debug
*/
