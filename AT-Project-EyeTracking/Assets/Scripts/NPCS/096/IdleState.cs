using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class IdleState : States
{
    public TriggeredState triggeredState;

    public GameObject player;
    public GameObject face;
    public GameObject self;

    Vector3 viewportPoint;

    public NavMeshAgent agent;

    public bool playerInFront;
    public bool isVisible;
    public bool lookingAtFace;

    RaycastFromEyes raycastFromEyes;

    public Transform centrePoint;

    public float range;

    public Animator animator;
    private void Start()
    {
        raycastFromEyes = player.GetComponentInChildren<RaycastFromEyes>();
    }

    public override States RunCurrentState()
    {
        agent.speed = 1f;
        Patrol();
        if (TriggerBooleanChecks())
        {

            animator.SetTrigger("TriggeredPanic");
            
            return triggeredState;
        }
        else
        {
            return this;
        }
    }

    public bool TriggerBooleanChecks()
    {
        viewportPoint = player.GetComponentInChildren<Camera>().WorldToViewportPoint(face.transform.position);
        isVisible = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;

        Vector3 dirToPlayer = (player.transform.position - self.transform.position).normalized;

        float dot = Vector3.Dot(self.transform.forward, dirToPlayer);

        playerInFront = dot > 0;

        if (raycastFromEyes.hasSeenFace == true)
        {

            lookingAtFace = true;
        }
        else
        {
            lookingAtFace = false;
        }

        if (isVisible && lookingAtFace && playerInFront)
        {
            return true;
        }
        return false;
    }
    public void Patrol()
    {
        if (TriggerBooleanChecks())
        {
            agent.SetDestination(centrePoint.position);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            animator.SetBool("IsWalking", false);
            var value = Random.Range(0, 100);
            var roll = Random.Range(0, 100);
            if (roll == value)
            {
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    animator.SetBool("IsWalking", true);
                    var timeToWait = 20f;
                    timeToWait -= Time.deltaTime;
                    if(timeToWait < 0)
                    {
                        agent.SetDestination(centrePoint.position);
                    }
                }
            }
            else
            {
                roll = Random.Range(0, 100);
            }

        }

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

}
