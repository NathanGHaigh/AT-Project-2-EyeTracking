using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    
    [SerializeField] Camera playerCamera;

    [SerializeField] float mouseSensitivity = 100f;

    [SerializeField] float cameraBounce;

    PlayerController playerController;

    float yRotation = 0f;

    float xRotation = 0f;

    private void Awake()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }     
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = playerInput.actions["Look"].ReadValue<Vector2>().x;
        float mouseY = playerInput.actions["Look"].ReadValue<Vector2>().y;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }

}
