using System;

public static class Blocks {
    public enum BlockId: byte {
        Air     = 0,
        Stone   = 1,
        Dirt    = 2,
        Grass   = 3,
        Bedrock = 4,
        Bricks  = 5,
        Planks  = 6,
        Stone_Bricks = 7,
        Wool = 8,
        Books = 9,
        Debug = 10,
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
        models[(int)BlockId.Planks]  = new Model(new ModelFace(7));
        models[(int)BlockId.Stone_Bricks]  = new Model(new ModelFace(8));
        models[(int)BlockId.Wool]  = new Model(new ModelFace(9));
        models[(int)BlockId.Books]  = new Model(new ModelFace(7), new ModelFace(7), new ModelFace(10));
        models[(int)BlockId.Debug]  = new Model(new ModelFace(11));

        blocks[(int)BlockId.Air]     = new Block("Air", false, models[(int)BlockId.Air]);
        blocks[(int)BlockId.Stone]   = new Block("Stone", true, models[(int)BlockId.Stone]);
        blocks[(int)BlockId.Dirt]    = new Block("Dirt", true, models[(int)BlockId.Dirt]);
        blocks[(int)BlockId.Grass]   = new Block("Grass", true, models[(int)BlockId.Grass]);
        blocks[(int)BlockId.Bedrock] = new Block("Bedrock", true, models[(int)BlockId.Bedrock]);
        blocks[(int)BlockId.Bricks]  = new Block("Bricks", true, models[(int)BlockId.Bricks]);
        blocks[(int)BlockId.Planks]  = new Block("Planks", true, models[(int)BlockId.Planks]);
        blocks[(int)BlockId.Stone_Bricks]  = new Block("Stone Bricks", true, models[(int)BlockId.Stone_Bricks]);
        blocks[(int)BlockId.Wool]  = new Block("Wool", true, models[(int)BlockId.Wool]);
        blocks[(int)BlockId.Books]  = new Block("Books", true, models[(int)BlockId.Books]);
        blocks[(int)BlockId.Debug]  = new Block("Debug", true, models[(int)BlockId.Debug]);
    }
}

/*
    Blocks:
    Enthält ein enum welches die Block sorte als byte angibt. Enthält alle Models sowie Blöcke im Spiel.
    Blöcke:
        Air, Stone, Dirt, Grass, Debug
*/
