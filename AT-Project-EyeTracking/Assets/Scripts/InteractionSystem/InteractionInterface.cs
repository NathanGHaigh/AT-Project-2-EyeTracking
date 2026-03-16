using UnityEngine.UI;
using System;
using UnityEngine;

namespace Interaction
{
    public enum Type
    {
        Interact,
        Pickup,
    };
    public interface IInteractable
    {
        public string MessageInteract { get; }
        public Type Type { get; }
        void Interact(InteractableControl interactableControl);
    }
}
