using Interaction;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

    [SerializeField] private bool IsSprinting;

    [SerializeField] private float stamina;

    [SerializeField] private float maxStamina;

    [SerializeField] private float minStamina;

    [SerializeField] private Slider staminaSlider;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] public GameObject manager;

    [SerializeField] public Item heldItem;

    [SerializeField] public Image equippedItem;

    [SerializeField] public AudioManager audioManager;


    public bool inventoryActive = false;

    public GameObject InventoryUI;

    public bool documentUIActive = false;

    public GameObject DocumentUI;
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

        if(audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }

        stamina = 10;

        staminaSlider = GameObject.Find("StaminaBar").GetComponent<Slider>();
        staminaSlider.maxValue = maxStamina;
        staminaSlider.minValue = minStamina;
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["SprintStart"].performed += x => SprintStart();
        playerInput.actions["SprintEnd"].performed += x => SprintEnd();
        playerInput.actions["Interact"].performed += OnInteract;
        playerInput.actions["Interact"].canceled += OnInteract;
        playerInput.actions["ToggleInventory"].performed += ToggleInventory;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Interact"].performed -= OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventoryActive)
        {
            InventoryUI.SetActive(false);
        }
        else
        {
            InventoryUI.SetActive(true);
        }

        if(!documentUIActive)
        {
            DocumentUI.SetActive(false);
        }
        else
        {
            DocumentUI.SetActive(true);
        }

        Move(MoveInput);
        CheckGrounded();
        ApplyGravity();
        Sprint();
    }

    private void FixedUpdate()
    {
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        IsMoving = context.ReadValue<Vector2>() != Vector2.zero;
        MoveInput = context.ReadValue<Vector2>();
    }

    private void SprintStart()
    {

            IsSprinting = true;
    }

    private void SprintEnd()
    {
        IsSprinting = false;
    }
    private void Sprint()
    {
        staminaSlider.value = stamina;
        if (IsSprinting)
        {
            moveSpeed = 6;
            stamina -= Time.deltaTime;
        }
        else
        {
            moveSpeed = 2;
            if(stamina >= maxStamina)
            {
                stamina = maxStamina;
            }
            else
                stamina += Time.deltaTime;
        }

        if (stamina <= 0)
        {
            IsSprinting = false;
        }
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Called Interact");
        manager.GetComponent<InteractableControl>().OnInteract();
        if(heldItem != null) 
            ResetHeldItem();
        if(documentUIActive)
        {
            documentUIActive = false;
            audioManager.PaperSelect();
        }
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        Debug.Log("PressedToggleInventory");
        if (!inventoryActive)
        {
            inventoryActive = true;
        }
        else
        {
            inventoryActive = false;

        }
    }

    private void ResetHeldItem()
    {
        heldItem = null;
        equippedItem.enabled = false;
        equippedItem.sprite = null;

    }
    #region To fix error: Ambiguous invocation of OnMove(InputAction.CallbackContext) and OnMove(InputValue)
    private void OnMove(InputValue value)
    {
        //To fix error: Ambiguous invocation of OnMove(InputAction.CallbackContext) and OnMove(InputValue)
    }

    private void OnInteract(InputValue value) { }

    private void OnSprint(InputValue value) { }
    #endregion
}
