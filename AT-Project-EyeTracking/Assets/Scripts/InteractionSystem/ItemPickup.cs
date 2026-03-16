using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{

    public class ItemPickup : MonoBehaviour, IInteractable
    {

        [SerializeField]
        Item Item;
        public Inventory inventory;

        AudioManager manager;

        public GameObject parent;

        void Start()
        {
            inventory = FindAnyObjectByType<Inventory>();
            manager = FindAnyObjectByType<AudioManager>();
        }
        public string MessageInteract => "Press E to Pick Up";

        public Type Type => Type.Pickup;

        public void Interact(InteractableControl interactableControl)
        {
            var emptySlot = inventory.inventorySlots.Find(slot => !slot.HasItem());
            if (emptySlot != null)
            {
                Debug.Log($"Adding Item{Item}");
                manager.PlayPickUpAudio();
                inventory.AddItem(Item);
                this.enabled = false;
                Destroy(parent);
            }
            else
            {
                Debug.Log("No Free Space");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
