using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
	public bool IsInteractable = true;

	private bool _isActiveInteractable = false;
	public bool IsActiveInteractable
	{
		get => _isActiveInteractable;
		set
		{
			if (value == _isActiveInteractable)
			{
				return;
			}

			_isActiveInteractable = value;
			RaiseActiveInteractableChange();
		}
	}

	private InteractablesController InteractablesController;

	// Start is called before the first frame update
	void Start()
	{
		InteractablesController = GetComponentInParent<InteractablesController>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		var playerController = collision.GetComponent<PlayerController>();
		if (playerController == null) return;

		// Event when player enters this object
		
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		var playerController = collision.GetComponent<PlayerController>();
		if (playerController == null) return;

		// Event when player leaves this object
		
	}

	public void SetActiveInteractable(bool active)
	{
		IsActiveInteractable = active;
	}

	public void RaiseActiveInteractableChange()
	{
		if (IsActiveInteractable) 
		{
			// Events when this object becomes the active interactable

			// select highlight
			// send player active info
		}
		else 
		{
			// Events when this object loses the active interactable

			// deselect highlight
			// send player active info
		}
	}
}
