using UnityEngine;
using static UnityEngine.GraphicsBuffer;

enum WatchState
{
    Watched,
    UnWatched
}

public class SCP689 : MonoBehaviour
{
    [SerializeField] WatchState state;

    GameObject player;

    RaycastFromEyes raycastFromEyes;

    EncounterManager encounterManager;

    public float watchedTime;

    public float lookAwayGracePeriod = 5;

    public float watchedQuota = 10;

    private void Awake()
    {
        raycastFromEyes = FindAnyObjectByType<RaycastFromEyes>();
        encounterManager = FindAnyObjectByType<EncounterManager>();
        player = GameObject.Find("Player 1");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                    WatchedState();
                }
                break;

            case WatchState.UnWatched:
                {
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
        if(lookAwayGracePeriod < 0 )
        {
            Debug.Log("DEAD");
        }
    }
}
