using UnityEngine;

namespace Interaction
{
    public class DoorButton : MonoBehaviour, IInteractable
    {
        public GameObject player;
        public float coolDownTimer;
        ItemKeyCard item; // Changed from Item to ItemKeyCard

        public GameObject linkedDoor;
        public string MessageInteract => "Press E to Open";

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
                Debug.Log("Opening Door");

                linkedDoor.GetComponent<Door>().Interacted(item);
                coolDownTimer = 1;
            }
        }

        public void Update()
        {
            if (coolDownTimer > 0)
            {
                coolDownTimer -= Time.deltaTime;
            }
            if (coolDownTimer < 0)
            {
                coolDownTimer = 0;
            }
        }
    }
}

