using UnityEngine;

public class SCP096Audio : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip idle;
    public AudioClip trigger;
    public AudioClip build;
    public AudioClip rageScream;

    public StateManager stateManager;
    private States lastState;
    private Coroutine triggerRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateManager = this.GetComponent<StateManager>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateManager == null)
            return;

        States current = stateManager.currentState;
        if (current == lastState)
            return; // no change

        // stop any running trigger sequence if state changed
        if (triggerRoutine != null)
        {
            StopCoroutine(triggerRoutine);
            triggerRoutine = null;
        }

        if (current is IdleState)
        {
            Idle();
        }
        else if (current is TriggeredState triggered) // pattern matching gives access to TriggeredState fields
        {
            // you can read fields from the actual instance without creating one:
            // float timeLeft = triggered.countDown;  // example usage
            TriggerRage();
        }
        else if (current is ChaseState chase)
        {
            audioSource.loop = true;
        }


            lastState = current;

    }

    void Idle()
    {
        audioSource.clip = idle;
        audioSource.loop = true;
        audioSource.Play();
    }

    void TriggerRage()
    {
        audioSource.loop = false;
        audioSource.clip = trigger;
        audioSource.PlayOneShot(trigger);

        Invoke(nameof(BuildUp), 2);
    }
    void BuildUp()
    {
        audioSource.clip = build;
        audioSource.loop = false;
        audioSource.Play();

        Invoke(nameof(Scream), 30);

    }   
    
    void Scream()
    {
        audioSource.clip = rageScream;
        audioSource.loop = true;
        audioSource.Play();
    }
}
