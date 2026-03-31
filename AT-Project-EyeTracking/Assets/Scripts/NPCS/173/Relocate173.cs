using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Relocate173 : MonoBehaviour
{
    public GameObject SCP173GO;

    SCP173 scp173;

    private List<GeneralWaypoint> allWaypoints = new();

    public GameObject player;

    public Camera playerCamera;

    public float timeCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player 1");

        scp173 = FindAnyObjectByType<SCP173>();

        allWaypoints = FindObjectsByType<GeneralWaypoint>(FindObjectsSortMode.None).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        Check173ForRelocate();        
    }

    void Check173ForRelocate()
    {
        if(scp173 != null)
        {
            if (scp173.lastSawPlayer > timeCheck)
            {
                Debug.Log("Attempting 173 Relocate");
                var selectedWaypoint = SelectRelocateWaypoint() as GeneralWaypoint;
                if (selectedWaypoint != null)
                {
                    Debug.Log("Teleporting SCP173 To" + selectedWaypoint);
                    Teleport173(selectedWaypoint.transform.position);
                    scp173.hasEverSeenPlayer = false;
                    scp173.lastSawPlayer = 0;
                    scp173.audioTrigger = true;
                }
                else
                {
                    Debug.Log("Failed Relocate");
                }
            }
        }
    }

    public GeneralWaypoint SelectRelocateWaypoint()
    {
        Vector3 playerPosition = player.transform.position;

        List<GeneralWaypoint> waypoints = allWaypoints
            .Where(p =>
             {
                 float distance = Vector3.Distance(playerPosition, p.transform.position);
                 return distance < 30;
             })
            .Where(p =>
            {
                Vector3 directionToPlayer = (playerPosition - p.transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(p.transform.position, playerPosition);
            
                if (Physics.Raycast(p.transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    return !hit.collider.CompareTag("Player");
                }
                return false;
            })
            .ToList();
        if(waypoints.Count <= 0)
        {
            return null;
        }

        return waypoints[0];
    }

    public void Teleport173(Vector3 pos)
    {
        SCP173GO.GetComponentInChildren<NavMeshAgent>().Warp(pos);
    }
}
