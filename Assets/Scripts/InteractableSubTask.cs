using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSubTask : MonoBehaviour
{
	public GameObject SubGroup;
	public InteractableSubTaskController InteractableSubTaskController;

	public void SubTask_OnClick() 
	{
		InteractableSubTaskController.DisableActiveSubGroup();
		InteractableSubTaskController.EnableActiveSubGroup(SubGroup);
	}

}
