using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField]
    bool isOpen;
    [SerializeField]
    bool isOpening;
    [SerializeField]
    NavMeshObstacle doorCollider;
    [SerializeField]
    GameObject doorMesh;

    public void Start()
    {
        isOpen = false;   
    }

    public void Interacted()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else if (!isOpen)
        {
            OpenDoor();
        }
    }
    public void OpenDoor()
    {
        doorMesh.SetActive(false);
        doorCollider.enabled = false;
        isOpen = true;
    }

    public void CloseDoor()
    {
        doorMesh.SetActive(true);
        doorCollider.enabled = true;
        isOpen = false;
    }
}
