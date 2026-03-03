using UnityEngine;

namespace Interaction
{
    public class TestInteractable : MonoBehaviour, IInteractable
    {
        public string MessageInteract => "This is a test interactable object.";
        public void Interact(InteractableControl interactableControl)
        {
            Debug.Log("Interacted with the test interactable object!");
        }
    }
}
