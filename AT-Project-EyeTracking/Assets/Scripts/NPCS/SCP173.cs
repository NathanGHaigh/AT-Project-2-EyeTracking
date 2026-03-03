using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SCP173 : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 30f; 
    [SerializeField] private float rotationSpeed = 720f; 

    [SerializeField] private float teleportStepDistance = 5f; 
    [SerializeField] private float teleportCooldown = 0.5f; 

    [SerializeField] private float minApproachDistance = 1f; 

    [SerializeField] private GameObject player; 

    [SerializeField] private Vector3 initialPosition; 
                        
    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Rigidbody rb;

    RaycastFromEyes raycastFromEyes;

    BlinkController blinkController;

    float teleportTimer = 0f;

    void Start()
    {
        if (raycastFromEyes == null && player != null)
        {
            raycastFromEyes = player.GetComponentInChildren<RaycastFromEyes>();
        }

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.autoBraking = false;
            agent.speed = moveSpeed;
        }

        if (blinkController == null && player != null)
        {
            blinkController = player.GetComponentInChildren<BlinkController>();
        }

        if (rb != null)
        {    
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || agent == null)
            return;

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

        Vector3 flatPlayerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 flatDirection = flatPlayerPos - transform.position;

        float distance = flatDirection.magnitude;

        if (distance <= minApproachDistance)
            return;

        teleportTimer -= Time.deltaTime;
        if (blinkController.isHoldingBlink)
        {
            teleportCooldown = 0.1f; 
        }
        else
        {
            teleportCooldown = 0.5f; 
        }
        if (teleportTimer <= 0f)
        {
            teleportTimer = teleportCooldown;

            float step = Mathf.Min(teleportStepDistance, distance - minApproachDistance);
            Vector3 candidate = transform.position + flatDirection.normalized * step;


            NavMeshPath path = new NavMeshPath();
            if(agent.CalculatePath(candidate, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
            }
        }

        if (flatDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
