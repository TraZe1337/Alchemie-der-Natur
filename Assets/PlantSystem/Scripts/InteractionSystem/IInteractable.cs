using UnityEngine;

public interface IInteractable
{
    void Interact(InteractionType type, PlayerInteraction interactor);
}