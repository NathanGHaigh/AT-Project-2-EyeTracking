using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private bool isGrounded;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector2 MoveInput;

    [SerializeField] public float Health = 100f;

    [SerializeField] public bool IsMoving;

    [SerializeField] private PlayerInput playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        Move(MoveInput);
        CheckGrounded();
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        IsMoving = context.ReadValue<Vector2>() != Vector2.zero;
        MoveInput = context.ReadValue<Vector2>();
    }

    private void Move(Vector3 moveDirection)
    {
        Vector3 motion = transform.forward * MoveInput.y + transform.right * MoveInput.x;
        motion.y = 0f;
        motion.Normalize();
        characterController.Move(motion * moveSpeed * Time.deltaTime);

    }

    private void CheckGrounded()
    {
        isGrounded = characterController.isGrounded;
    }
    private void ApplyGravity()
    {
        Vector3 gravityVector = Vector3.zero;
        if (!isGrounded)
        {
            gravityVector.y -= gravity * Time.deltaTime;
        }
        characterController.Move(gravityVector);
    }

    #region To fix error: Ambiguous invocation of OnMove(InputAction.CallbackContext) and OnMove(InputValue)
    private void OnMove(InputValue value)
    {
        //To fix error: Ambiguous invocation of OnMove(InputAction.CallbackContext) and OnMove(InputValue)
    }
    #endregion
}
