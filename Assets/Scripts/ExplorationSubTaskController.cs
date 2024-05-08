using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationSubTaskController : MonoBehaviour
{
    public ExplorationController ExplorationController;

	private InteractableSubTaskController interactableSubTaskController;

	private void Awake()
	{
		ExplorationController = FindObjectOfType<ExplorationController>();
		interactableSubTaskController = GetComponent<InteractableSubTaskController>();
	}

	public void ExploreDisco(InteractionScriptableObject interaction)
	{
		interactableSubTaskController.TriggerInteraction(interaction);
		ExplorationController.StartExploring(ExplorationController.ExploreType.Disco);
	}

	public void ExploreLibrary(InteractionScriptableObject interaction)
	{
		interactableSubTaskController.TriggerInteraction(interaction);
		ExplorationController.StartExploring(ExplorationController.ExploreType.Library);
	}
}
