using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class DoorButton : MonoBehaviour, IInteractable
    {
        public GameObject player;

        public Material active;
        public Material inActive;

        public GameObject panel;
        public GameObject button;

        public float coolDownTimer;

        public int timeToSet;

        public Animator animator;
        Vector3 origin;
        ItemKeyCard item; // Changed from Item to ItemKeyCard

        public GameObject linkedDoor;
        public string MessageInteract => "Press E to Open";

        public Type Type => Type.Interact;

        public AudioSource source;

        public AudioClip buttonPress;

        public void Awake()
        {
            origin = button.transform.position;

            player = GameObject.Find("Player 1");
        }
        public void Interact(InteractableControl interactableControl)
        {
            if (coolDownTimer == 0)
            {
                if (player.GetComponent<PlayerController>().heldItem != null && player.GetComponent<PlayerController>().heldItem is ItemKeyCard)
                {
                    item = player.GetComponent<PlayerController>().heldItem as ItemKeyCard;
                }
                else
                {
                    item = null;
                }
                source.clip = buttonPress;
                source.Play();
                Debug.Log("Opening Door");
                animator.SetTrigger("Press");
                linkedDoor.GetComponent<Door>().Interacted(item);
                coolDownTimer = timeToSet;
            }
        }

        public void Update()
        {
            if (coolDownTimer > 0)
            {
                panel.GetComponent<Renderer>().material = active;
                gameObject.GetComponent<Collider>().enabled = false;
                coolDownTimer -= Time.deltaTime;
            }
            if (coolDownTimer < 0)
            {
                panel.GetComponent<Renderer>().material = inActive;
                gameObject.GetComponent<Collider>().enabled = true;
                coolDownTimer = 0;
            }
        }
    }
}

