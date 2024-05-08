using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MovementSpeed = 1.0f;

	public StatusController StatusController;
	private readonly List<GameObject> _interactableGameObjects = new List<GameObject>();

	public GameObject SubTaskCenter;
	public TimeController TimeController;

	private bool _isInteracting = false;

	public GameObject Base;
	public GameObject Shirt;
	public GameObject Shoes;
	public GameObject Hair;
	public GameObject Pants;

	List<Animator> _playerAnimators = new List<Animator>();

	private Rigidbody2D _rigidBody;

	// Start is called before the first frame update
	void Start()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_playerAnimators.Add(Base.GetComponent<Animator>());
		_playerAnimators.Add(Shirt.GetComponent<Animator>());
		_playerAnimators.Add(Shoes.GetComponent<Animator>());
		_playerAnimators.Add(Hair.GetComponent<Animator>());
		_playerAnimators.Add(Pants.GetComponent<Animator>());
	}

	// Update is called once per frame
	void Update()
	{
		MovementUpdate();
		InteractablesUpdate();
		InputUpdate();
	}

	public void LoadCharacterisation(Dictionary<string, object> data)
	{
		Base.GetComponent<SpriteRenderer>().color = (Color)data["BaseColor"];
		Shirt.GetComponent<SpriteRenderer>().color = (Color)data["ShirtColor"];
		Shoes.GetComponent<SpriteRenderer>().color = (Color)data["ShoeColor"];
		Hair.GetComponent<SpriteRenderer>().color = (Color)data["HairColor"];
		Pants.GetComponent<SpriteRenderer>().color = (Color)data["PantsColor"];
		Hair.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)data["HairAC"];
		Pants.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)data["PantsAC"];
	}

	bool _isPlayerMoving = false;
	int _playerDirectionFacing = 1; // -1 left : 1 right
	private void MovementUpdate()
	{
		// Can't move while interacting.
		if (_isInteracting)
		{
			return;
		}

		int deltaMove = 0;

		if (Input.GetKey(KeyCode.A) && !_isTouchingLeftWall) // Could be mapped to movement keys
		{
			deltaMove -= 1;
		}
		if (Input.GetKey(KeyCode.D) && !_isTouchingRightWall) // Could be mapped to movement keys
		{
			deltaMove += 1;
		}

		SetPlayerWalkingAnimation(deltaMove != 0);

		// Check player facing direction
		if (deltaMove != 0 && deltaMove != _playerDirectionFacing)
		{
			_playerDirectionFacing = deltaMove;
			SetPlayerAnimationDirection(deltaMove);
		}

		var movementSpeedModifier = 1f;
		if (StatusController.HasBadStatus())
		{
			movementSpeedModifier -= 0.5f;
		}

		var horizontalMoveDistance = deltaMove * MovementSpeed * movementSpeedModifier;
		_rigidBody.velocity = new Vector3(horizontalMoveDistance, 0f, 0f);
	}

	private bool _isTouchingRightWall = false;
	private bool _isTouchingLeftWall = false;
	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("Player:OnTriggerEnter");
		switch (other.tag)
		{
			case "RightWall":
				_isTouchingRightWall = true;
				return;
			case "LeftWall":
				_isTouchingLeftWall = true;
				return;			
		}
	}
	public void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Player:OnCollisionEnter2D");
		switch (collision.gameObject.tag)
		{
			case "RightWall":
				_isTouchingRightWall = true;
				return;
			case "LeftWall":
				_isTouchingLeftWall = true;
				return;
		}
	}
	public void OnTriggerExit(Collider other)
	{
		Debug.Log("Player:OnTriggerExit");
		switch (other.tag)
		{
			case "RightWall":
				_isTouchingRightWall = false;
				return;
			case "LeftWall":
				_isTouchingLeftWall = false;
				return;
		}
	}
	public void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log("Player:OnCollisionExit2D");
		switch (collision.gameObject.tag)
		{
			case "RightWall":
				_isTouchingRightWall = false;
				return;
			case "LeftWall":
				_isTouchingLeftWall = false;
				return;
		}
	}

	private void SetPlayerWalkingAnimation(bool isWalking)
	{
		_playerAnimators.ForEach(a => { a.SetBool("IsWalking", isWalking); });
	}

	private void SetPlayerAnimationDirection(int moveDirection)
	{
		transform.localScale = new Vector3(moveDirection, 1, 1);
		//_playerAnimators.ForEach(a => { a.transform.rotation = new Quaternion(0, 90 + -90 * moveDirection, 0, 0); });
	}

	private InteractableBase _currentActiveInteractable = null;
	private void InteractablesUpdate()
	{
		if (_interactableGameObjects.Count == 0)
		{
			// if we have an active interactable and we leave the last one we have
			// we want to turn the interactable off and remove it.
			if (_currentActiveInteractable != null)
			{
				_currentActiveInteractable.SetActiveInteractable(false);
				_currentActiveInteractable = null;
			}

			return;
		}

		GameObject closestInteractable = null;
		foreach (var interactable in _interactableGameObjects)
		{
			// set first interactable as closest
			if (closestInteractable == null)
			{
				closestInteractable = interactable;
				continue;
			}

			// if next interactable is closer than current interactable set as current
			if (GetDistanceFromObject(interactable) < GetDistanceFromObject(closestInteractable))
			{
				closestInteractable = interactable;
			}
		}

		var interactableBase = closestInteractable.GetComponent<InteractableBase>();
		if (interactableBase == null)
		{
			throw new Exception($"InteractableBase not found in object={closestInteractable} even though it is in list of interactable gameobjects");
		}

		if (interactableBase == _currentActiveInteractable)
			return;

		Debug.Log("New Active Interactable found, setting active to new interactable.");

		// turn off old, turn on new
		_currentActiveInteractable?.SetActiveInteractable(false);
		interactableBase.SetActiveInteractable(true);
		Debug.Log("ActiveInteractableSet");
		_currentActiveInteractable = interactableBase;
	}

	private const KeyCode InteractKey = KeyCode.F;
	private GameObject _interactMenu = null;
	private InteractableSubTaskController _subTaskController = null;
	private void InputUpdate()
	{
		if (_isInteracting)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				StopInteracting();
			}

			return;
		}

		if (Input.GetKeyDown(InteractKey))
		{
			if (_currentActiveInteractable == null)
			{
				Debug.Log("No object nearby to interact with.");
				return;
			}

			Debug.Log($"Interacting with {_currentActiveInteractable.gameObject.name}.");
			_interactMenu = Instantiate(_currentActiveInteractable.InteractionMenu, SubTaskCenter.transform, false);
			_subTaskController = _interactMenu.GetComponent<InteractableSubTaskController>();
			_subTaskController.Player = this;

			_isInteracting = true;
		}
	}

	public void Interact_OnGameTick(int timePassedPerTick)
	{
		Debug.Log($"Interact_OnGameTick({timePassedPerTick})");
		StatusController.Interact(_interaction, timePassedPerTick);
		if (_interaction.HasMaximumTime && _interaction.CurrentMaximumTime <= 0)
		{
			Debug.Log("Interaction has reached maximum time. Stopping Interaction");
			StopInteracting();
		}
	}

	public bool CanInteract(InteractionScriptableObject interaction)
	{
		return StatusController.CanInteract(interaction);
	}

	private InteractionScriptableObject _interaction;
	public void StartInteracting(InteractionScriptableObject interaction)
	{
		Debug.Log($"StartInteracting({interaction.name})");
		_interaction = interaction;
		if (interaction.Money < 0)
			StatusController.AcceptCost(interaction.Money);
		TimeController.OnGameTick += Interact_OnGameTick;
		_subTaskController = null;
		Destroy(_interactMenu);
		_interactMenu = null;
	}

	public void StopInteracting()
	{
		Debug.Log("StopInteracting()");
		TimeController.OnGameTick -= Interact_OnGameTick;
		_interaction = null;
		_isInteracting = false;
		_subTaskController = null;
		Destroy(_interactMenu);
		_interactMenu = null;
	}

	/// <summary>
	/// Gets the absolute distance from a gameobject to this object.
	/// </summary>
	/// <param name="gameObject">The object to get the distance from</param>
	/// <returns>The absolute distance</returns>
	private float GetDistanceFromObject(GameObject gameObject)
	{
		return Math.Abs(this.transform.position.x - gameObject.transform.position.x);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("OnTriggerEnter2D()");
		var interactable = collision.GetComponent<InteractableBase>();
		if (interactable != null)
		{
			Debug.Log($"Interactable {interactable.gameObject.name} collided.");

			_interactableGameObjects.Add(collision.gameObject);

			return;
		}

		// other object checks.
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		var interactable = collision.GetComponent<InteractableBase>();
		if (interactable != null)
		{
			_interactableGameObjects.Remove(collision.gameObject);

			return;
		}

		// other object checks.
	}
}
