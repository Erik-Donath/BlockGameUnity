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

/*
 Block:
    Hat die Eigenschaften: Solid, Name, Model.
    Solid: gibt an ob der Block Solide ist.
    Name: Name des Blocks.
    Model: Das Model des Blocks
*/