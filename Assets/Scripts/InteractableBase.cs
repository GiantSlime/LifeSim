using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
	public bool IsInteractable = true;
	private SpriteRenderer _spriteRenderer;

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

	// Start is called before the first frame update
	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
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

			_spriteRenderer.color = Color.blue;
		}
		else 
		{
			// Events when this object loses the active interactable

			_spriteRenderer.color = Color.white;
		}
	}
}
