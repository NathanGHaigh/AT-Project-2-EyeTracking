using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ChaseState : States
{
    public GameObject Player;
    public GameObject self;
    public NavMeshAgent agent;

    public Animator animator;
    public override States RunCurrentState()
    {
        agent.speed = 10f;
        agent.autoBraking = false;
        RunAtPlayer();
        InAttackRange();
        return this;
    }

    public void RunAtPlayer()
    {
        agent.SetDestination(Player.transform.position);
        Vector3 rayOrigin = self.transform.position;
        Vector3 rayDirection = self.transform.forward;
  
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin, rayDirection, out hit, 1.5f)) 
        {
            Debug.Log("Door Detection Casting");
            if(hit.collider.gameObject.tag == "Door")
            {

                Debug.Log("Open Door");
                hit.collider.gameObject.GetComponentInChildren<Door>().OpenDoor();
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
            animator.SetBool("InAttackRange", true);
            return true;
        }

        animator.SetBool("InAttackRange", false);
        return false;

    }
}
