public struct ModelFace {
    public int Texture {
        get; private set;
    }
    public ModelFace(int texture) {
        this.Texture = texture;
    }
}

public class Model {
    public ModelFace NorthFace { get => Faces[0]; private set => Faces[0] = value; }
    public ModelFace SouthFace { get => Faces[1]; private set => Faces[1] = value; }
    public ModelFace WestFace  { get => Faces[2]; private set => Faces[2] = value; }
    public ModelFace EastFace  { get => Faces[3]; private set => Faces[3] = value; }
    public ModelFace UpFace    { get => Faces[4]; private set => Faces[4] = value; }
    public ModelFace DownFace  { get => Faces[5]; private set => Faces[5] = value; }

    public ModelFace[] Faces {
        get; private set;
    }

    public Model(ModelFace[] faces) {
        if(faces.Length != 6)
            throw new System.ArgumentException("Faces must be of Lenght 6");
        Faces = faces;
    }
    public Model(ModelFace face) {
        Faces = new ModelFace[6] { face, face, face, face, face, face };
    }
    public Model(ModelFace up, ModelFace down, ModelFace sides) {
        Faces = new ModelFace[6] { sides, sides, sides, sides, up, down };
    }
}

/*
 Model:
 Hier wird das Model mit 6 Seiten dargestellt.
 
 ModelFace:
 Abstraction um eine Seite des Models darzustellen.
*/