using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSubTaskController : MonoBehaviour
{
	public PlayerController Player;

	public void Start()
	{
		var subtasks = GetComponentsInChildren<InteractableSubTaskOption>(includeInactive:true);
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

	private GameObject activeSubGroup;
	public void DisableActiveSubGroup()
	{
		activeSubGroup?.SetActive(false);
	}

	public void EnableActiveSubGroup(GameObject subGroup)
	{
		activeSubGroup = subGroup;
		activeSubGroup.SetActive(true);
	}
}
