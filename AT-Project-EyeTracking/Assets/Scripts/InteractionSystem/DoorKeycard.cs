using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class DoorKeycard: MonoBehaviour, IInteractable
    {
        public GameObject player;

        public Material active;
        public Material inActive;

        public GameObject panel;

        public float coolDownTimer;

        public float timeToSet;

        ItemKeyCard item; // Changed from Item to ItemKeyCard

        public GameObject linkedDoor;
        public string MessageInteract => "Needs a Level " + linkedDoor.GetComponent<Door>().levelAccess + " Keycard to Operate";
        public Type Type => Type.Interact;

        public AudioSource source;

        public AudioClip buttonPress;

        public AudioClip keycardUseSuccess;

        public AudioClip keycardUseFail;

        public TextMeshProUGUI interactionText;

        public float interactionTime;

        public void Interact(InteractableControl interactableControl)
        {

            if (coolDownTimer == 0)
            {
                if (player.GetComponent<PlayerController>().heldItem != null && player.GetComponent<PlayerController>().heldItem is ItemKeyCard)
                {
                    item = player.GetComponent<PlayerController>().heldItem as ItemKeyCard;
                    if (item.AccessLevel >= linkedDoor.GetComponent<Door>().levelAccess)
                    {
                        source.clip = keycardUseSuccess;
                        source.Play();
                        interactionText.SetText("Access Granted");
                        interactionTime = 5;
                        linkedDoor.GetComponent<Door>().Interacted(item);
                        coolDownTimer = timeToSet;
                    }
                    else
                    {
                        interactionText.SetText(MessageInteract);
                        interactionTime = 5;
                        source.clip = keycardUseFail;
                        source.Play();
                    }
                }
                else
                {
                    item = null;
                    source.clip = buttonPress;
                    interactionText.SetText("Needs a Keycard to Operate");
                    interactionTime = 5;
                    source.Play();
                }

            }
        }

        public void Update()
        {
            if (coolDownTimer > 0)
            {
                panel.GetComponent<Renderer>().material = active;
                coolDownTimer -= Time.deltaTime;
            }
            if (coolDownTimer < 0)
            {
                panel.GetComponent<Renderer>().material = inActive;
                coolDownTimer = 0;
            }

            if(interactionTime > 0)
            {
                interactionTime -= Time.deltaTime;
            }
            if(interactionTime < 0) 
            {
                interactionTime = 0;
                interactionText.SetText("");
            }
        }
    }
}

