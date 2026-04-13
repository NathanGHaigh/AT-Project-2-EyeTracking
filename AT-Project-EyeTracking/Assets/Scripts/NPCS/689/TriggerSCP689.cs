using UnityEngine;

public class TriggerSCP689 : MonoBehaviour
{
    [SerializeField] private GameObject scp689Prefab; // Reference to the SCP-689 prefab

    EncounterManager encounterManager; // Reference to the EncounterManager

    AudioManager audioManager; // Reference to the AudioManager

    Collider triggerCollider; // Reference to the trigger collider

    BlinkController blinkController; // Reference to the BlinkController

    bool hasBeenTriggered = false;

    float shortDelay = 3f;

    void Awake()
    {
        encounterManager = FindAnyObjectByType<EncounterManager>(); // Get the EncounterManager instance
        audioManager = FindAnyObjectByType<AudioManager>(); // Get the AudioManager instance
        blinkController = FindAnyObjectByType<BlinkController>(); // Get the BlinkController instance
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBeenTriggered)
        {
                shortDelay -= Time.deltaTime;
            if (shortDelay <= 0)
            {
                blinkController.ForceBlink();
                Destroy(this.gameObject);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            StartTriggerSCP689();
        }
    }

    private void StartTriggerSCP689()
    {
        hasBeenTriggered = true;
        audioManager.Play689Spawn();
        if (encounterManager != null)
        {
            encounterManager.is689Active = true;   
        }
        else
        {
            Debug.LogWarning("EncounterManager or SCP-689 prefab is not assigned.");
        }
    }
}
