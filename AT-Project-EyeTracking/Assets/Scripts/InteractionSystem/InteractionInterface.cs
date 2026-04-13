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
        string MessageInteract { get; }
        Type Type { get; }
        void Interact(InteractableControl interactableControl);
    }
}
