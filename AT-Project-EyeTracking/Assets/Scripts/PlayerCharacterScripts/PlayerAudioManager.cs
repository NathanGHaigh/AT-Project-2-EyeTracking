using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip stepWalk;

    public AudioClip stepRun;

    public float rateOfStepWalk = 0.5f;

    public float rateOfStepRun = 0.3f;

    float stepTimer = 0f;

    PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerController.isMoving)
        {
            if (stepTimer > 0f)
            {
                stepTimer -= Time.deltaTime;
                return;
            }
            else
            {
                stepTimer = 0f;
                return;
            }
        }

        float rateOfStep = playerController.IsSprinting ? rateOfStepRun : rateOfStepWalk;
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            if(playerController.IsSprinting)
            {
                PlayRunStep();
            }
            else
            {
                PlayWalkStep();
            }
            stepTimer = rateOfStep; 
        }
    }

    void PlayWalkStep()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(stepWalk);
    }
    void PlayRunStep()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(stepRun);
    }
}
