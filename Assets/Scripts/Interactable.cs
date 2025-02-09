using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //Use to add or remove an Event component to this gameobject.
    public bool useEvents;

    // message displayed to player when looking at an interactable.
    public string promptMessage;



    // Function called from player
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }

    protected virtual void Interact()
    {
        // No code. It is a tempate to be overriden by subclasses.
    }
}
