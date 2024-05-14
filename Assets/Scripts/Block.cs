public class Block {
    public string Name {
        get; protected set;
    }
    public bool Solid {
        get; protected set; 
    }
    public BlockStates States {
        get; protected set;
    }

    private Block(string name, bool solid, BlockStates states) {
        Name = name;
        Solid = solid;
        States = states;
    }

    public override string ToString() {
        return Name;
    }

    public static BlockStates AirStates = new BlockStates(new VoxelModel(new VoxelTextures(0)));
    public static BlockStates DirtStates = new BlockStates(new VoxelModel(new VoxelTextures(1)));
    public static BlockStates GrasStates = new BlockStates(new VoxelModel(new VoxelTextures(3, 1, 2)));

    public static Block[] Blocks {
        get; private set;
    } = new Block[] {
        new Block("Air",    false, AirStates),
        new Block("Dirt",   true,  DirtStates),
        new Block("Grass",  true,  GrasStates),
    };
}
