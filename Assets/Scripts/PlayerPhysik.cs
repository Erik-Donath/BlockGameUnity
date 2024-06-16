using UnityEngine;

public class PlayerPhysik : MonoBehaviour {
    [SerializeField] private Vector3Int boundOffset = new Vector3Int(-1, -1, -1);
    [SerializeField] private Vector3Int boundSize = new Vector3Int(3, 5, 3);
    [SerializeField] private Vector3 boundCellSize = new Vector3(1, 1, 1);
    [SerializeField] private LayerMask ground;
    [SerializeField] private PhysicMaterial groundMaterial;

    private BoxCollider[,,] coliders;

    void Start() {
        coliders = new BoxCollider[boundSize.x, boundSize.y, boundSize.z];
        for(int x = 0; x < boundSize.x; x++) {
            for(int y = 0; y < boundSize.y; y++) {
                for(int z = 0; z < boundSize.z; z++) {
                    BoxCollider colider = gameObject.AddComponent<BoxCollider>();
                    coliders[x, y, z] = colider;
                    colider.enabled = false;
                    colider.center = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) + boundOffset;
                    colider.includeLayers = ground;
                    colider.material = groundMaterial;
                    colider.size = boundCellSize;
                }
            }
        }
    }

    void Update() {
        Vector3Int pos = World.Instance.PlayerBlockCoord;
        transform.position = pos;
        for(int x = 0; x < boundSize.x; x++) {
            for(int y = 0; y < boundSize.y; y++) {
                for(int z = 0; z < boundSize.z; z++) {
                    coliders[x, y ,z].enabled = World.Instance.IsSolid(pos + boundOffset + new Vector3Int(x, y, z));
                }
            }
        }
    }
}
