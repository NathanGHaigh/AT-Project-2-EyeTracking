using Interaction;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    [SerializeField] private float sprintSpeed = 6f;

    [SerializeField] private float stamina;

    [SerializeField] private float maxStamina;

    [SerializeField] private float minStamina;

    [SerializeField] private Slider staminaSlider;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] public GameObject manager;

    [SerializeField] public Item heldItem;

    [SerializeField] public Image equippedItem;

    [SerializeField] public AudioManager audioManager;

    [SerializeField] public bool isDead = false;

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
        //audioManager.PlayLoopedAudio(audioManager.scp096Audio, audioManager.scp096Slash2);

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

        if (!isDead)
        {
            Move(MoveInput);
            CheckGrounded();
            ApplyGravity();
            Sprint();
        }
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
            moveSpeed = sprintSpeed;
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
        if(!isDead)
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

    public void Kill(int DeathID)
    {
        StartCoroutine(DeathSequence(DeathID));
    }

    private IEnumerator DeathSequence(int DeathID)
    {
        if(DeathID == 1)
        {
            sprintSpeed = 2.5f;
            audioManager.PlayLoopedAudio(audioManager.playerAlt, audioManager.heartBeatFast, 0.7f); 
            yield return StartCoroutine(HeartAttack());
            yield return StartCoroutine(FallOver());
            SceneManager.LoadScene("DeathScene");

        }
        else if(DeathID == 2)
        {

            audioManager.PlayNeckSnap();
            yield return StartCoroutine(FallOver());


            Debug.Log("Player has died");
            SceneManager.LoadScene("DeathScene");

        }
        else if(DeathID == 3)
        {
            yield return StartCoroutine(FallOver());
            audioManager.PlayLoopedAudio(audioManager.scp096Audio, audioManager.scp096Slash2);
        }
    }

    private IEnumerator HeartAttack()
    {

        float elapsed = 0f;
        float heartAttackDuration = 10f;


        while (elapsed < heartAttackDuration)
        { 
            elapsed += Time.deltaTime;
            float t = elapsed / heartAttackDuration;


            
            stamina -= Time.deltaTime * 4;
            if(stamina < 0f) stamina = 0f;

            moveSpeed = Mathf.Lerp(moveSpeed, 0f, t * 0.050f); 

            if(characterController.enabled) characterController.Move(Physics.gravity * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator FallOver()
    {
        isDead = true;
        characterController.enabled = false; 
        playerInput.enabled = false; 


        Quaternion startRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f); transform.rotation = startRot;

        Quaternion endRot = startRot * Quaternion.Euler(0f, 0f, 90f);


        Vector3 startPos = transform.position;

        Vector3 endPos = startPos - transform.up * 0.1f;

        this.GetComponentInChildren<CameraController>().enabled = false;

        float elapsed = 0f;
        while (elapsed < 2.5f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / 2.5f);

            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            float angle = t * 90f * Mathf.Deg2Rad;
            float drop = 0.6f * (1f - Mathf.Cos(angle));
            transform.position = startPos - Vector3.up * drop;
            yield return null;
        }

        transform.rotation = endRot;
        yield return new WaitForSeconds(1f); 
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
