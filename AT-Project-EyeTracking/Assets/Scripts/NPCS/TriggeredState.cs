using UnityEngine;

public class TriggeredState : States
{
    public ChaseState chaseState;

    public bool activated;
    public Animator animator;

    public float countDown = 60f;

    public AudioManager audioManager;

    public void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    public override States RunCurrentState()
    {
        CountDown();
        if (activated)
        {
            audioManager.Play096Chase();
            return chaseState;
        }
        return this;
    }

    public void CountDown()
    {
        activated = false;
        countDown -= Time.deltaTime;
        if(countDown < 1)
        {
            animator.SetTrigger("PanicEnd");
        }

        if(countDown <= 0)
        {
            
            activated = true;
        }

    }
}
