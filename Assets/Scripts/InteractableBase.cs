using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
	public bool IsInteractable = true;
	private SpriteRenderer _spriteRenderer;
	private PlayerController _playerController;

	public Sprite DefaultSprite;
	public Sprite InteractedSprite;

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

	public GameObject InteractionMenu;

	// Start is called before the first frame update
	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_playerController = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		// Event when player enters this object
		
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
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

			_spriteRenderer.sprite = InteractedSprite;
		}
		else 
		{
			// Events when this object loses the active interactable

			_spriteRenderer.sprite = DefaultSprite;
		}
	}

	public void OnMouseEnter()
	{
		IsActiveInteractable = true;
	}

	public void OnMouseOver()
	{
		IsActiveInteractable = true;
	}

	public void OnMouseExit()
	{
		IsActiveInteractable = false;
	}

	public void OnMouseDown()
	{
		if (_playerController == null)
		{
			Debug.Log("Player controller is null. Cannot resolve MouseDown event");
			return;
		}

		_playerController.MovePlayerTo(this.transform.position);
	}
}
