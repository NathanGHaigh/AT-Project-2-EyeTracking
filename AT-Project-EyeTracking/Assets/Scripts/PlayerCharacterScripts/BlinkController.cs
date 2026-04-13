using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    [SerializeField] private float blinkDuration = 0.25f;

    [SerializeField] private float blinkInterval; 

    [SerializeField] private float maxBlinkInterval = 10f;

    [SerializeField] LinearProgressBar sliderBlink;

    [SerializeField] GameObject blinkImage;

    [SerializeField] private Image blinkIcon;

    [SerializeField] private Sprite eyeOpen, eyeClosed;

    [SerializeField] PlayerInput playerInput;

    [SerializeField] private PlayerController playerController;

    public bool isBlinking;

    public bool isHoldingBlink;
    private float blinkDurationRemaining;

    RaycastFromEyes raycastFromEyes;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = GetComponentInParent<PlayerInput>();
        }
        if(playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
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
        blinkDurationRemaining = blinkDuration;
        SliderSetup();
    }

    void Update()
    {
        if (!isBlinking)
            if(playerController.activeEyedrops)
            {
                blinkInterval -= Time.deltaTime * playerController.rateEyedrops;
            }
        blinkInterval -= Time.deltaTime;
        MangageBlinkSlider();
        UpdateBlinkIcon();
        BlinkManager();

    }

    public void StartBlink(bool held)
    {
        isBlinking = true;
        isHoldingBlink = held;
        blinkImage.SetActive(true); 
        raycastFromEyes.currentViewedObject = null; 
        if(raycastFromEyes.lookingAt173)
        {
            raycastFromEyes.lookingAt173 = false;
        }

        blinkInterval = 0f; 

        if(!held)
        {
            blinkDurationRemaining = blinkDuration; 
        }
    }

    private void StopBlink()
    {
        isBlinking = false;
        isHoldingBlink = false;
        blinkImage.SetActive(false);
        blinkDurationRemaining = blinkDuration; 
        blinkInterval = maxBlinkInterval; 
        blinkDuration = 0.25f;
    }
    private void SliderSetup()
    {
        if (sliderBlink == null)
        {
            sliderBlink = GetComponent<LinearProgressBar>();
            return;
        }
        sliderBlink.minimum = 0f;
        sliderBlink.maximum = 10f;
        blinkInterval = maxBlinkInterval;
    }
    private void MangageBlinkSlider()
    {
        if(sliderBlink == null)
        {
            return;
        }
        sliderBlink.currentValue = blinkInterval;
    }

    private void UpdateBlinkIcon()
    {
        if (blinkIcon == null)
            return;
        blinkIcon.sprite = isBlinking ? eyeClosed : eyeOpen;
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
            if (blinkInterval <= 0f)
            {
                Debug.Log("Blink!");
                StartBlink(held: false);
                //blinkInterval = 10f;
            }
        }
    }

        public void ForceBlink()
        {
            StartBlink(held: false);
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
