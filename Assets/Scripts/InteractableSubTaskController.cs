using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSubTaskController : MonoBehaviour
{
	public PlayerController Player;

	public GameObject SubTaskPanel;

	public void Start()
	{
		var subtasks = GetComponentsInChildren<InteractableSubTask>();
        foreach (var item in subtasks)
        {
            if (!Player.CanInteract(item.Interaction))
			{
				item.GetComponent<Button>().interactable = false;
			}
        }
    }

	public void TriggerInteraction(InteractionScriptableObject interaction)
	{
		Player.StartInteracting(interaction);
	}
}
