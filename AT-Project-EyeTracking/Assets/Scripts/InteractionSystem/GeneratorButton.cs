using UnityEngine;
using Interaction;

public class GeneratorButton : MonoBehaviour, IInteractable
{ 

    public string MessageInteract => "Press E to Activate Generator";

    public GameObject linkedGenerator;
    public Type Type => Type.Interact;

    public void Interact(InteractableControl interactableControl)
    {
        if(linkedGenerator.GetComponent<Generator>().isPowered == false)

            linkedGenerator.GetComponent<Generator>().TurnedOn();
            
    }
}
