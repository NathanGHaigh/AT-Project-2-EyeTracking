using System.ComponentModel;
using UnityEditor.Timeline;
using UnityEngine;

namespace Interaction
{

    public class ItemPickup : MonoBehaviour, IInteractable
    {

        [SerializeField]
        Item Item;
        [SerializeField]
        GameObject prefab;
        public Inventory inventory;
        [SerializeField]
        GameObject model;

        void Start()
        {
            inventory = FindAnyObjectByType<Inventory>();
            
            Instantiate(prefab, model.transform.position, transform.rotation);

        }
        public string MessageInteract => throw new System.NotImplementedException();

        public void Interact(InteractableControl interactableControl)
        {
            Debug.Log($"Adding Item{Item}");
            inventory.AddItem(Item);
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
