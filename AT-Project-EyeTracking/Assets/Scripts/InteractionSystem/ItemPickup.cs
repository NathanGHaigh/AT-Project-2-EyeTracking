using System.ComponentModel;
using UnityEditor.Timeline;
using UnityEngine;

namespace Interaction
{

    public class ItemPickup : MonoBehaviour, IInteractable
    {

        [SerializeField]
        Item Item;
        public Inventory inventory;

        void Start()
        {
            inventory = FindAnyObjectByType<Inventory>();
        }
        public string MessageInteract => "Press E to Pick Up";

        public void Interact(InteractableControl interactableControl)
        {
            Debug.Log($"Adding Item{Item}");
            inventory.AddItem(Item);
            this.enabled = false;
            Destroy(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
