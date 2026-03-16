using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class TestInteractable : MonoBehaviour, IInteractable
    {
        public string MessageInteract => "This is a test interactable object.";

        public Type Type => Type.Interact;

        public void Interact(InteractableControl interactableControl)
        {
            Debug.Log("Interacted with the test interactable object!");
        }
    }
}
