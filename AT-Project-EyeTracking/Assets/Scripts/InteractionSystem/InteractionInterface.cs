using System;

namespace Interaction
{ 
    public interface IInteractable
    {
        public string MessageInteract { get; }
        void Interact(InteractableControl interactableControl);
    }
}
