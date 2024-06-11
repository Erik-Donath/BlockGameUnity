using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameObject cam;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float gravity = 9.807f;

    [SerializeField] private float width  = 1.0f;
    [SerializeField] private float height = 2.0f;

    private bool isGrounded = false;
    private float h, v;
    private float mouseH, mouseV;

    public Vector3Int BlockPos {
        get {
            Vector3 pos = transform.position;
            return new Vector3Int((int)Mathf.Floor(pos.x), (int)Mathf.Floor(pos.y), (int)Mathf.Floor(pos.z));
        }
        private set {
            transform.position = value;
        }
    }

    private void Update() {
        GetInput();

        Vector3 velocity = ((transform.forward * v) + (transform.right * h)) * Time.deltaTime * speed;
        velocity += Vector3.up * -gravity * Time.deltaTime;
        if(checkDown(velocity.y)) velocity.y = 0.0f;

        transform.Rotate(Vector3.up * mouseH);
        cam.transform.Rotate(Vector3.right * -mouseV);

        transform.Translate(velocity, Space.World);
    }

    private void GetInput() {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseH = Input.GetAxis("Mouse X");
        mouseV = Input.GetAxis("Mouse Y");
    }

    private bool checkDown(float downSpeed = 1.0f) {
        Vector3Int pos = BlockPos;
        isGrounded = (
            World.Instance.IsSolid(new Vector3Int((int)(pos.x + width), (int)(pos.y - downSpeed), (int)(pos.z - width))) ||
            World.Instance.IsSolid(new Vector3Int((int)(pos.x - width), (int)(pos.y - downSpeed), (int)(pos.z - width))) ||
            World.Instance.IsSolid(new Vector3Int((int)(pos.x + width), (int)(pos.y - downSpeed), (int)(pos.z + width))) ||
            World.Instance.IsSolid(new Vector3Int((int)(pos.x - width), (int)(pos.y - downSpeed), (int)(pos.z + width)))
        );
        return isGrounded;
    }
}
