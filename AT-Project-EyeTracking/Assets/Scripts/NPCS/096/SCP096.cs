using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SCP096 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Checks for Face Seen
    Vector3 viewportPoint;
    public Camera playerCamera;
    public GameObject player;
    public GameObject face;
    public bool visible;
    public bool playerInFront;
    public bool lookingAtFace;
    public bool hasBeenTrigged;

    RaycastFromEyes raycastFromEyes;

    BlinkController blinkController;

    NavMeshAgent agent;

    void Start()
    {
        raycastFromEyes = player.GetComponentInChildren<RaycastFromEyes>();
        blinkController = player.GetComponentInChildren<BlinkController>();
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        viewportPoint = playerCamera.WorldToViewportPoint(face.transform.position);
        visible = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;

        Vector3 dirToPlayer = (player.transform.position - this.transform.position).normalized;

        float dot = Vector3.Dot(this.transform.forward, dirToPlayer);

        playerInFront = dot > 0;

        if (raycastFromEyes.currentViewedObject != null)
        {
            var viewed = raycastFromEyes.currentViewedObject;

            lookingAtFace = (viewed.tag == "096Face");
        }
        else
        {
            lookingAtFace = false;
        }
        if (Trigger096(visible, playerInFront, lookingAtFace))
        {
            hasBeenTrigged = true;
        }
        if (hasBeenTrigged)
        {
            PathToPlayer();           
        }
    }

    bool Trigger096(bool visible, bool playerInFront, bool lookingAtFace)
    {
        if (visible && playerInFront && lookingAtFace)
        {
            return true;
        }
        return false;
    }

    void PathToPlayer()
    {
        agent.destination = player.transform.position;
    }
}
