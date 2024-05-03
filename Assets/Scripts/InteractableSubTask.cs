using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSubTask : MonoBehaviour
{
    public InteractionScriptableObject Interaction;

	public InteractableSubTaskController _interactableSubTaskController;

	public void SubTask_OnClick() 
	{
		if (Interaction.HasMaximumTime)
			Interaction.CurrentMaximumTime = Interaction.MaximumTime;
		_interactableSubTaskController.TriggerInteraction(Interaction);
	}

}
