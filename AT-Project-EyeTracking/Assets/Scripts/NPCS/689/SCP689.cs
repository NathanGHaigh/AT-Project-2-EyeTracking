using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

enum WatchState
{
    Watched,
    UnWatched
}

public class SCP689 : MonoBehaviour
{
    [SerializeField] WatchState state;

    public bool isWatched;

    GameObject player;

    RaycastFromEyes raycastFromEyes;

    EncounterManager encounterManager;

    AudioManager audioManager;

    public float watchedTime;

    public float lookAwayGracePeriod = 5;

    public float watchedQuota = 10;

    public bool playerDead = false;

    private void Awake()
    {
        raycastFromEyes = FindAnyObjectByType<RaycastFromEyes>();
        encounterManager = FindAnyObjectByType<EncounterManager>();
        player = GameObject.Find("Player 1");
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager audioManager = FindAnyObjectByType<AudioManager>();
        audioManager.Play689Spawn();
        var lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (raycastFromEyes.watching689 == true)
        {
            state = WatchState.Watched;
        }
        else
        {
            state = WatchState.UnWatched;
        }
        ManageState();
        
    }

    void ManageState()
    {
        switch (state)
        {

            case WatchState.Watched:
                {
                    isWatched = true;
                    WatchedState();
                }
                break;

            case WatchState.UnWatched:
                {
                    isWatched = false;
                    UnWatchedState();
                }
                break;

        }

    }

    void WatchedState()
    {
        watchedTime += Time.deltaTime;
        if(watchedTime > watchedQuota)
        {
            AudioManager audioManager = FindAnyObjectByType<AudioManager>();
            audioManager.Play689Despawn();
            encounterManager.Despawn689(this.gameObject);

            Debug.Log("689 Despawn");
        }

    }

    void UnWatchedState()
    {
        if (lookAwayGracePeriod > 0)
        { 
            lookAwayGracePeriod -= Time.deltaTime;
        }
        if(lookAwayGracePeriod < 0 && !playerDead)
        {
            playerDead = true;
            player.GetComponentInChildren<PlayerController>().Kill(1);
            Debug.Log("DEAD");
        }
    }
}
