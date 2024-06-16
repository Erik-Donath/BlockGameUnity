using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviour {

    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [Space]
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseSensitivity = 100f;


    private Rigidbody rb;
    [SerializeField] private bool isGrounded;
    private float xRotation = 0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() {
        /*if(rb.velocity.y <= 0.01 && rb.velocity.y >= -0.01)
            Debug.Log("Ja");
        isGrounded = Physics.Raycast(transform.position + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down, groundCheckDistance, groundLayers);
        if(isGrounded)
            Debug.Log("Joa");*/
        isGrounded = rb.velocity.y <= 0.01 && rb.velocity.y >= -0.01;
        Look();
        Move();
        if(Input.GetButtonDown("Jump") && isGrounded) {
            Jump();
        }
    }

    private void Look() {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        cam.transform.Rotate(Vector3.right * -mouseY);
    }

    private void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        moveDirection.Normalize();

        if(Input.GetKey(KeyCode.LeftShift)) {
            rb.MovePosition(rb.position + moveDirection * speed * sprintMultiplier * Time.deltaTime);
        }
        else {
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }

        if(x == 0 && z == 0 && isGrounded) {
            rb.velocity = Vector3.zero;
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
