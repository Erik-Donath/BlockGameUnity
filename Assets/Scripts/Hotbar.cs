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
