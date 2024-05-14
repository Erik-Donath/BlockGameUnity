using BlockData;
using System.Collections.Generic;
using UnityEngine;

public interface IModel {
    public bool IsPrimitv { get; }
}

public class BlockStates {
    public IModel DefaultModel {
    get; private set; }

    public BlockStates(IModel defaultModel) {
        DefaultModel = defaultModel;
    }
}

public class VoxelModel : IModel {
    public bool IsPrimitv => true;
    public VoxelTextures Textures {
        get; private set;
    }

    public VoxelModel(VoxelTextures textures) {
        Textures = textures;
    }

    public List<VoxelFace> GetFaces(Vector3Int position, BlockNabours nabours) {
        List<VoxelFace> faces = new List<VoxelFace>();
        bool[] solids = nabours.Solids;
        int[] textures = Textures.Textures;

        for(int i = 0; i < 6; i++) {
            if(!solids[i])
                faces.Add(new VoxelFace(position, (FaceDirection)i, VoxelTextures.GetTexture(textures[i])));
        }
        return faces;
    }
}