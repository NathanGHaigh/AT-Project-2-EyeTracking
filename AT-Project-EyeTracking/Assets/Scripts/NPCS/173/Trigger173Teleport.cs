using UnityEngine;
using UnityEngine.AI;

public class Trigger173Teleport : MonoBehaviour
{
    public GameObject scp173;

    public Collider teleportTrigger;

    public GeneralWaypoint waypoint;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (scp173 != null && waypoint != null)
            {
                scp173.GetComponentInChildren<NavMeshAgent>().Warp(waypoint.transform.position);
                teleportTrigger.enabled = false;
                Destroy(this);
            }
        }

    }
}
