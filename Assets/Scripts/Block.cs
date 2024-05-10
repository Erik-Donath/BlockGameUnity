public class Block {
    public readonly string Name;
    public readonly Model Model;
    public readonly bool IsSolid;


    private Block(string name, Model model, bool isSolid) {
        this.Name = name;
        this.Model = model;
        this.IsSolid = isSolid;
    }

    public static readonly Block[] blocks = {
        new Block("Stone", new PrimitivModel(new PrimitivTextures(0)), true),           // 0
        new Block("Dirt",  new PrimitivModel(new PrimitivTextures(1)), true),           // 1
        new Block("Grass", new PrimitivModel(new PrimitivTextures(2, 1, 1)), true),     // 2
    };
}
