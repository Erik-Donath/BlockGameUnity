public class Block {
    public string Name {
        get; protected set;
    }
    public bool Solid {
        get; protected set; 
    }
    public Model Model {
        get; protected set;
    }

    public Block(string name, bool solid, Model states) {
        Name = name;
        Solid = solid;
        Model = states;
    }

    public override string ToString() {
        return Name;
    }
}
