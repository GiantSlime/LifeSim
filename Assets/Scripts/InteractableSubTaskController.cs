using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSubTaskController : MonoBehaviour
{
	public PlayerController Player;

	public void TriggerInteraction(InteractionScriptableObject interaction)
	{
		Player.StartInteracting(interaction);
	}
}
