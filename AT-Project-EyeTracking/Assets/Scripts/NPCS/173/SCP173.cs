using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SCP173 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float teleportStepDistance = 5f;
    [SerializeField] private float minApproachDistance = 1f;

    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Rigidbody rb;

    private RaycastFromEyes raycastFromEyes;
    private AudioManager audioManager;
    private BlinkController blinkController;
    private scp173Audio scp173Audio;
    private float teleportTimer = 0f;
    [SerializeField] private bool beingLookedAt = false;

    [SerializeField] private bool directLOS = false;

    [SerializeField] private LayerMask layersToAccount;

    [SerializeField] private bool hasSeenPlayer = false;

    [SerializeField] public bool hasEverSeenPlayer = false;

    [SerializeField] float range = 20;

    [SerializeField] public float lastSawPlayer = 0;

    [SerializeField] Vector3 lastPlayerPos = new();

    [SerializeField] Vector3 offset = new();

    [SerializeField] public bool audioTrigger = true;

    [SerializeField] Vector3 randomRoamPoint;

    [SerializeField] bool hasRoamPoint = false;

    public bool isMoving = false;

    void Start()
    {
        if (player != null)
        {
            raycastFromEyes = player.GetComponentInChildren<RaycastFromEyes>();
            blinkController = player.GetComponentInChildren<BlinkController>();
            audioManager = FindAnyObjectByType<AudioManager>();
            scp173Audio = FindAnyObjectByType<scp173Audio>();
        }

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.autoBraking = false;
            agent.speed = moveSpeed;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (player == null || agent == null)
            return;

        if(player.GetComponentInChildren<PlayerController>().isDead)
        {
            hasSeenPlayer = false;
            hasEverSeenPlayer = false;
            lastSawPlayer = 0;
            audioTrigger = false;
            isMoving = false;
            scp173Audio.audioClip = null;
            scp173Audio.StopAudio();
            return;
        }

        if (!hasSeenPlayer)
        {
            RandomPointRoam();           
        }
        if(hasRoamPoint)
        {
            var time = 20f;
            
            time -= Time.deltaTime;
            if(time <= 0)
            {
                hasRoamPoint = false;
                RandomPointRoam();
            }
        }

        if (isMoving && agent.velocity.magnitude <= 0.1f)
        {
            isMoving = false;
        }

        if (isMoving)
        {
            scp173Audio.PlayAudio();
        }

        else
        {
            scp173Audio.StopAudio();
        }


        if (hasEverSeenPlayer)
        {
            if (hasSeenPlayer)
            {
                lastSawPlayer = 0;
            }
            else
            {
                lastSawPlayer += Time.deltaTime;
                if (lastSawPlayer > 60)
                {
                    if (!audioTrigger)
                        audioTrigger = true;
                }
            }
        }

        beingLookedAt = BeingLookedAt();
        directLOS = HasDirectLos();

        var DistancetoPlayer = Vector3.Distance(this.gameObject.transform.position, player.gameObject.transform.position);
        //Debug.Log(DistancetoPlayer);


        if (DistancetoPlayer < range && directLOS)
        {
            lastPlayerPos = player.transform.position;
            hasSeenPlayer = true;
            hasEverSeenPlayer = true;
            // Check if being looked at

            if (beingLookedAt)
            {
                isMoving = false;
                teleportTimer = 0f;
                agent.isStopped = true;
                if(DistancetoPlayer < 10 && directLOS && audioTrigger)
                {
                    Debug.Log("Audio Played");
                    audioManager.PlaySCP173Audio();
                    audioTrigger = false;
                }
                return;
            }

            if(DistancetoPlayer <= 3.5f)
            {
                //Debug.Log("Killed Player");
                KillPlayer();
                return;
            }
            isMoving = true;
            Move(lastPlayerPos);
        }

        else
        {
            if (hasSeenPlayer && !beingLookedAt)
            {
                Move(lastPlayerPos);
                isMoving = true;
            }
        }
    }

    private bool HasDirectLos()
    {
        if (Physics.Raycast(transform.position + offset, (player.gameObject.transform.position + offset - transform.position), out RaycastHit hitInfo, range, layersToAccount))
        {
            if(hitInfo.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position + offset, hitInfo.point - transform.position, Color.yellow);
                return true;
            }
            Debug.DrawRay(transform.position + offset, hitInfo.point - transform.position, Color.red);
        }
        return false;
    }

    private bool BeingLookedAt()
    {
        return raycastFromEyes.lookingAt173;
    }

    void Move(Vector3 pos)
    {
        if(agent.isStopped)
            agent.isStopped = false;

        // Flat distance
        Vector3 flatPlayerPos = new Vector3(pos.x, transform.position.y, pos.z);
        Vector3 flatDirection = flatPlayerPos - transform.position;

        if (flatDirection.magnitude <= minApproachDistance)
            if (!directLOS)
            {
                hasSeenPlayer = false;
                return;
            }

        agent.SetDestination(pos);
        isMoving = true;

        // Rotate to face player
        if (flatDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void RandomPointRoam()
    {
        var chance = Random.Range(1, 1000);
        var roll = Random.Range(1, 1000);
        if(hasRoamPoint)
        {
            Vector3 flatDir = new Vector3(randomRoamPoint.x, transform.position.y, randomRoamPoint.z);
            if(Vector3.Distance(flatDir, transform.position) <= 2f)
            {
                hasRoamPoint = false;
            }
            else
            {
                Move(randomRoamPoint);
                return;
            }

        }

        if (chance == roll)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 25;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 25, NavMesh.AllAreas))
            {
                randomRoamPoint = hit.position;
                hasRoamPoint = true;
                roll = Random.Range(1, 1000);
            }
        }
        else
        {
            roll = Random.Range(1, 1000);
        }

    }
    void KillPlayer()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        player.GetComponentInChildren<PlayerController>().Kill(2);
    }
}