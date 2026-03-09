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
    Animator doorAnim;
    public void Start()
    {
        isOpen = false;
    }

    public void Update()
    {

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
        isOpen = true;
        doorAnim.SetTrigger("Open");

    }

    public void CloseDoor()
    {
        isOpen = false;
        doorAnim.SetTrigger("Close");

    }
}
