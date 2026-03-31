using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ChaseState : States
{
    public GameObject Player;
    public GameObject self;
    public NavMeshAgent agent;

    public Animator animator;

    AudioManager audioManager;

    bool hasKilledPlayer = false;
    bool killTriggered = false;
    public override States RunCurrentState()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        agent.speed = 10f;
        agent.autoBraking = false;
        RunAtPlayer();
        InAttackRange();
        if(hasKilledPlayer && !killTriggered)
        {
            killTriggered = true;
            Player.GetComponent<PlayerController>().Kill(3);
            audioManager.PlaySCP096Slash();
        }
        return this;
    }

    public void RunAtPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(Player.transform.position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(Player.transform.position);
        }
        else if (path.status == NavMeshPathStatus.PathPartial)
        {
            Vector3 bestPoint = path.corners[path.corners.Length - 1];
            agent.SetDestination(bestPoint);
        }

        Debug.Log($"Path Status: {agent.pathStatus} | Has Path: {agent.hasPath} | Pending: {agent.pathPending}");

        Vector3 rayOrigin = self.transform.position;
        Vector3 rayDirection = self.transform.forward;
  
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin, rayDirection, out hit, 1.5f)) 
        {
            Debug.Log("Door Detection Casting");
            if(hit.collider.gameObject.tag == "Door")
            {

                Debug.Log("Open Door");
                if (hit.collider.gameObject.GetComponentInChildren<Door>() != null)
                {
                    hit.collider.gameObject.GetComponentInChildren<Door>().OpenDoor();
                }
                Debug.DrawRay(rayOrigin, rayDirection, Color.green, 1.5f);
            }
            else
            {
                Debug.Log(hit.collider.tag);
                Debug.Log(hit.collider.gameObject.name);
                Debug.Log("No Door to open");
                Debug.DrawRay(rayOrigin, rayDirection, Color.red, 1.5f);
            }
        }
    }

    public bool InAttackRange()
    {   
        float distanceToPlayer = Vector3.Distance(Player.transform.position, self.transform.position);
        //Debug.Log(distanceToPlayer);
        if(distanceToPlayer < 2)
        {
            agent.speed = 0f;
            animator.SetBool("InAttackRange", true);
            hasKilledPlayer = true;
            return true;
        }

        animator.SetBool("InAttackRange", false);
        return false;

    }
}
