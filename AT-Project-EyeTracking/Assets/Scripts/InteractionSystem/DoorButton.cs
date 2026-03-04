using UnityEngine;

namespace Interaction
{
    public class DoorButton : MonoBehaviour, IInteractable
    {

        public GameObject linkedDoor;
        public string MessageInteract => "Button Pressed";

        public void Interact(InteractableControl interactableControl)
        {
            Debug.Log("Opening Door");

            linkedDoor.GetComponent<Door>().Interacted();
        }
    }
}

