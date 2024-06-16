using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [Space]
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseSensitivity = 100f;

    private CharacterController cc;
    private Vector3 velocity;

    private void Start() {
        cc = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        Look();
        Move();
        
    }

    private void Look() {
        float mouseH = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseV = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cam.transform.position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 rot = cam.transform.localEulerAngles;
        rot += new Vector3(-mouseV, mouseH, 0.0f);
        //rot.x = Mathf.Clamp(rot.x % 360, -90, 90);
        rot.z = 0;

        cam.transform.localEulerAngles = rot;
        transform.localEulerAngles = new Vector3(0.0f, rot.y, 0.0f);
    }

    private void Move() {
        velocity = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        velocity.Normalize();
        velocity *= speed * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftShift)) {
            velocity *= sprintMultiplier;
        }

        cc.Move(velocity);
    }
}
