using UnityEngine;
using Interaction;

public class GeneratorButton : MonoBehaviour, IInteractable
{ 

    public string MessageInteract => "Press E to Activate Generator";

    public GameObject linkedGenerator;
    public GameObject tv1;
    public GameObject tv2;

    public Collider trigger096173;
    public Type Type => Type.Interact;
    public Material active;
    public MeshRenderer panel;
    public MeshRenderer screen2;

    public AudioSource generator;
    public AudioSource button;

    public void Interact(InteractableControl interactableControl)
    {
        if (linkedGenerator.GetComponent<Generator>().isPowered == false)
        {

            linkedGenerator.GetComponent<Generator>().TurnedOn();
            panel.material = active;
            tv1.SetActive(false);
            tv2.SetActive(true);
            trigger096173.enabled = true;
            generator.Play();
            button.Play();
        }
        
            
    }
}
