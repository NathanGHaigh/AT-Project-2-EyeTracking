using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class DoorKeycard: MonoBehaviour, IInteractable
    {
        public GameObject player;

        public Material active;
        public Material inActive;
        public Material Broken;

        public GameObject panel;

        public float coolDownTimer;

        public float timeToSet;

        ItemKeyCard item; // Changed from Item to ItemKeyCard

        public GameObject linkedDoor;
        public string MessageInteract { get; set; }
        public Type Type { get; } = Type.Interact;

        public AudioSource source;

        public AudioClip buttonPress;

        public AudioClip keycardUseSuccess;

        public AudioClip keycardUseFail;

        public TextMeshProUGUI interactionText;

        public float interactionTime;

        public void Awake()
        {
            player = GameObject.Find("Player 1");

            interactionText = GameObject.Find("IndicatorText").GetComponent<TextMeshProUGUI>();

            UpdatePanel();
        }
        public void Interact(InteractableControl interactableControl)
        {

            if (coolDownTimer == 0)
            {
                if (player.GetComponent<PlayerController>().heldItem != null && player.GetComponent<PlayerController>().heldItem is ItemKeyCard)
                {
                    if(linkedDoor.GetComponent<Door>().stateID() == 1)
                    {
                        source.clip = buttonPress;
                        interactionText.SetText("Nothing happens, must be broken");
                        interactionTime = 5;
                        source.Play();
                        return;
                    }
                    if(linkedDoor.GetComponent<Door>().stateID() == 2)
                    {
                        source.clip = buttonPress;
                        interactionText.SetText("Needs Power to Operate");
                        interactionTime = 5;
                        source.Play();
                        return;
                    }




                    item = player.GetComponent<PlayerController>().heldItem as ItemKeyCard;
                    if (item.AccessLevel >= linkedDoor.GetComponent<Door>().levelAccess)
                    {
                        source.clip = keycardUseSuccess;
                        source.Play();
                        interactionText.SetText("Access Granted");
                        interactionTime = 5;
                        linkedDoor.GetComponent<Door>().Interacted(item);
                        coolDownTimer = 2;
                    }
                    else
                    {
                        interactionText.SetText("Need a level " + linkedDoor.GetComponent<Door>().levelAccess + " Keycard to Operate");
                        interactionTime = 5;
                        source.clip = keycardUseFail;
                        source.Play();
                    }
                }
                else
                {
                    item = null;
                    source.clip = buttonPress;
                    if(linkedDoor.GetComponent<Door>().stateID() == 1)
                    {
                        interactionText.SetText("Nothing happens, must be broken");
                    }
                    else if(linkedDoor.GetComponent<Door>().stateID() == 2)
                    {
                        interactionText.SetText("Needs Power to Operate");
                    }
                    else
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


        public void UpdatePanel()
        {
            var stateID = linkedDoor.GetComponent<Door>().stateID();
            if (stateID == 0)
            {
                panel.GetComponent<Renderer>().material = inActive;
                MessageInteract = "Needs a Keycard to Operate";
            }
            else if (stateID == 1)
            {
                panel.GetComponent<Renderer>().material = Broken;
                MessageInteract = "Nothing happens, must be broken";
            }
            else if (stateID == 2)
            {
                panel.GetComponent<Renderer>().material = Broken;
                MessageInteract = "Needs Power to Operate";
            }
        }
    }
}

