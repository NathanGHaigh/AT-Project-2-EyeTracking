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
    private BlinkController blinkController;
    private float teleportTimer = 0f;

    void Start()
    {
        if (player != null)
        {
            raycastFromEyes = player.GetComponentInChildren<RaycastFromEyes>();
            blinkController = player.GetComponentInChildren<BlinkController>();
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

        // Check if being looked at
        bool beingLookedAt = false;
        if (raycastFromEyes != null && raycastFromEyes.currentViewedObject != null)
        {
            var viewed = raycastFromEyes.currentViewedObject;
            beingLookedAt = (viewed == gameObject) || viewed.transform.IsChildOf(transform);
        }

        if (beingLookedAt)
        {
            teleportTimer = 0f;
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;

        // Flat distance check to avoid jitter at close range
        Vector3 flatPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 flatDirection = flatPlayerPos - transform.position;

        if (flatDirection.magnitude <= minApproachDistance)
            return;

        float cooldown = (blinkController != null && blinkController.isHoldingBlink) ? 0.1f : 1.5f;
        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            teleportTimer = cooldown;
            TeleportAlongPath();
        }

        // Rotate to face player
        if (flatDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void TeleportAlongPath()
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = agent.CalculatePath(player.transform.position, path);

        if (!hasPath || path.corners.Length < 2)
            return;

        if (path.status != NavMeshPathStatus.PathComplete && path.status != NavMeshPathStatus.PathPartial)
            return;

        float remainingStep = teleportStepDistance;
        Vector3 warpTarget = path.corners[0];

        for (int i = 1; i < path.corners.Length; i++)
        {
            float segmentLength = Vector3.Distance(warpTarget, path.corners[i]);

            if (segmentLength <= remainingStep)
            {
                remainingStep -= segmentLength;
                warpTarget = path.corners[i];
            }
            else
            {
                warpTarget = Vector3.MoveTowards(warpTarget, path.corners[i], remainingStep);
                break;
            }
        }

        float distToPlayer = Vector3.Distance(warpTarget, player.transform.position);
        if (distToPlayer < minApproachDistance)
        {
            Vector3 dirAway = (warpTarget - player.transform.position).normalized;
            warpTarget = player.transform.position + dirAway * minApproachDistance;
        }

        if (NavMesh.SamplePosition(warpTarget, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }
}