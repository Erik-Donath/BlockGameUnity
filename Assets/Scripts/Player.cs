using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [Space]
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseSensitivity = 100f;
    [Space]
    [SerializeField] private GameObject highlight;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private float highlightStep = 0.25f;
    [SerializeField] private float maxHightLightDistance = 5.0f;
    [SerializeField] private float placeDestroyCooldown = 1.0f;

    private CharacterController cc;
    private Vector3 velocity;
    private Vector3Int destroyPos;
    private Vector3Int placePos;
    private float nextPlaceDestroyTime;

    private void Start() {
        cc = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        nextPlaceDestroyTime = Time.time;
    }

    private void Update() {
        Look();
        Move();
        UpdateHightLight();
        if(!(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
            nextPlaceDestroyTime = Time.time - 1.0f;
        if(nextPlaceDestroyTime <= Time.time && SetDestroyBlock()) {
            nextPlaceDestroyTime = Time.time + placeDestroyCooldown;
        }
    }

    private void Look() {
        float mouseH = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseV = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        Vector3 rot = cam.transform.localEulerAngles + transform.localEulerAngles;
        rot += new Vector3(-mouseV, mouseH, 0.0f);
        //rot.z = 0;

        cam.transform.localEulerAngles = new Vector3(rot.x, 0.0f, 0.0f);
        transform.localEulerAngles = new Vector3(0.0f, rot.y, 0.0f);
    }

    private void Move() {
        velocity = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.Space)) {
            velocity.y += 1;
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            velocity.y -= 1;
        }

        velocity.Normalize();
        velocity *= speed * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftControl)) {
            velocity *= sprintMultiplier;
        }

        cc.Move(velocity);
    }

    private void UpdateHightLight() {
        float step = 0;
        Vector3 pos = cam.transform.position;
        placePos = new Vector3Int(-1, -1, -1);
        while(step <= maxHightLightDistance) {
            Vector3Int blockPos = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
            if(World.Instance.IsSolid(blockPos)) {
                highlight.transform.position = blockPos + new Vector3(0.5f, 0.5f, 0.5f);
                highlight.SetActive(true);
                destroyPos = blockPos;
                return;
            }
            placePos = blockPos;
            pos += cam.transform.forward * highlightStep;
            step += highlightStep;
        }

        highlight.SetActive(false);
        highlight.transform.position = new Vector3(0.5f, 0.5f, 0.5f);
        destroyPos = placePos = new Vector3Int(-1, -1, -1);
        highlight.SetActive(true);
    }
    private bool SetDestroyBlock() {
        if(Input.GetKey(KeyCode.Mouse1)) {
            World.Instance.SetBlock(placePos, hotbar.SelectedBlockId);
            return true;
        }
        if(Input.GetKey(KeyCode.Mouse0)) {
            World.Instance.SetBlock(destroyPos, Blocks.BlockId.Air);
            return true;
        }
        return false;
    }
}
