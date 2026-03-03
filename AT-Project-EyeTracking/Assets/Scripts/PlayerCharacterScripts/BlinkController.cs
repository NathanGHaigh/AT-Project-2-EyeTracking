using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    [SerializeField] private float blinkDuration = 0.1f; // Duration of the blink in seconds

    [SerializeField] private float blinkInterval; // Time between blinks in seconds

    [SerializeField] private float maxBlinkInterval = 10f; // Maximum time between blinks in seconds

    [SerializeField] Slider sliderBlink;

    [SerializeField] GameObject blinkImage;

    [SerializeField] PlayerInput playerInput;

    public bool isBlinking;

    public bool isHoldingBlink;
    private float blinkDurationRemaining;

    RaycastFromEyes raycastFromEyes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = GetComponentInParent<PlayerInput>();
        }
        playerInput.actions["Blink"].started += OnBlinkStarted;
        playerInput.actions["Blink"].canceled += OnBlinkCanceled;
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Blink"].started -= OnBlinkStarted;
            playerInput.actions["Blink"].canceled -= OnBlinkCanceled;
        }
    }
    private void OnBlinkStarted(InputAction.CallbackContext context)
    {
        StartBlink(held: true);

    }

    private void OnBlinkCanceled(InputAction.CallbackContext context)
    {
        StopBlink();
    }


    void Start()
    {
        if(raycastFromEyes == null)
        {
            raycastFromEyes = GetComponentInParent<RaycastFromEyes>();
        }
        blinkDurationRemaining = blinkDuration; // Initialize the blink duration remaining
        SliderSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBlinking)
            blinkInterval += Time.deltaTime;
        MangageBlinkSlider();
        BlinkManager();

    }

    private void StartBlink(bool held)
    {
        isBlinking = true;
        isHoldingBlink = held;
        blinkImage.SetActive(true); // Show the blink image
        raycastFromEyes.currentViewedObject = null; // Clear the currently viewed object during the blink

        blinkInterval = 0f; // Reset the blink interval when a blink starts

        if(!held)
        {
            blinkDurationRemaining = blinkDuration; // Set the blink duration for a normal blink
        }
    }

    private void StopBlink()
    {
        isBlinking = false;
        isHoldingBlink = false;
        blinkImage.SetActive(false); // Hide the blink image
        blinkDurationRemaining = blinkDuration; // Reset the blink duration for the next blink
        blinkInterval = 0f; // Reset the blink interval for the next blink
    }
    private void SliderSetup()
    {
        if (sliderBlink == null)
        {
            sliderBlink = GetComponent<Slider>();
            return;
        }
        sliderBlink.minValue = 0f;
        sliderBlink.maxValue = maxBlinkInterval;
    }
    private void MangageBlinkSlider()
    {
        if(sliderBlink == null)
        {
            return;
        }
        sliderBlink.value = blinkInterval;
    }

    private void BlinkManager()
    {
        if (isBlinking)
        {
            if (!isHoldingBlink)
            {
                blinkDuration -= Time.deltaTime;
                if (blinkDuration <= 0f)
                {
                    StopBlink();
                }
            }
        }
        else
        {
            if (blinkInterval >= maxBlinkInterval)
            {
                // Trigger the blink
                Debug.Log("Blink!");
                StartBlink(held: false);
                blinkInterval = 0f; // Reset the blink interval
            }
        }
    }

    #region InputSystem inputvalues
    public void OnBlink(InputValue value)
    {

    }
    public void OnBlinkCancelled(InputValue value)
    {

    }
    #endregion
}
