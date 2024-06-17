using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hotbar : MonoBehaviour {
    [SerializeField] private List<GameObject> items;
    [SerializeField] private GameObject selectImage;
    private int sel;

    public int Selected {
        get {
            return sel;
        }
        set {
            Select(value);
        }
    }
    public Blocks.BlockId SelectedBlockId {
        get {
            switch(Selected) {
                case 0:
                    return Blocks.BlockId.Dirt;
                case 1:
                    return Blocks.BlockId.Grass;
                case 2:
                    return Blocks.BlockId.Stone;
                case 3:
                    return Blocks.BlockId.Bedrock;
                case 4:
                    return Blocks.BlockId.Bricks;
                case 5:
                    return Blocks.BlockId.Planks;
                case 6:
                    return Blocks.BlockId.Stone_Bricks;
                case 7:
                    return Blocks.BlockId.Wool;
                case 8:
                    return Blocks.BlockId.Books;
                default:
                    return Blocks.BlockId.Air;
            }
        }
    }

    private void Start() {
        Select(8);
    }

    private void Update() {
        float v = Input.GetAxis("Mouse ScrollWheel");
        if(v > 0) Selected++;
        if(v < 0) Selected--;
    }

    public void Select(int v) {
        if(v < 0) v = items.Count + (v % items.Count);
        sel = v % items.Count;
        selectImage.transform.position = items[sel].transform.position;
    }
}
