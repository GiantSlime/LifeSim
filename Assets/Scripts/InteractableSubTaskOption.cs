using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSubTaskOption : MonoBehaviour
{
    // The actual interaction that happens.
    public InteractionScriptableObject Interaction;

    public InteractableSubTaskController InteractableSubTaskController;

    public void ClickOption()
    {
        InteractableSubTaskController.TriggerInteraction(Interaction);
    }
}
