using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float MovementSpeed = 1.0f;

	public StatusController StatusController;
	private readonly List<GameObject> _interactableGameObjects = new List<GameObject>();

	public GameObject SubTaskCenter;
	public TimeController TimeController;

	public bool _isInteracting = false;
	public bool IsInteracting
	{
		get { return _isInteracting; }
		set
		{
			_isInteracting = value;
			InventoryController.InventoryButton.interactable = !value && !_isExploring;
		}
	}

	public bool _isExploring = false;
	public bool IsExploring {
		get
		{
			return _isExploring;
		}
		set
		{
			_isExploring = value;
			InventoryController.InventoryButton.interactable = !value && !_isInteracting;
		}
	}
	public bool IsInventorying => InventoryController?.IsInventorying ?? false;

	public GameObject Base;
	public GameObject Shirt;
	public GameObject Shoes;
	public GameObject Hair;
	public GameObject Pants;

	List<Animator> _playerAnimators = new List<Animator>();

	private Rigidbody2D _rigidBody;

	public InventoryController InventoryController;
	[HideInInspector]
	public ObjectivesController ObjectivesController;
	[HideInInspector]
	public FoodController FoodController;

	public GameObject StopInteractingButton;
	public GameObject BackInteractionButton;

	public Animator BedAnim;
	public GameObject bedplayerhead;
	public GameObject playerBook;
	public GameObject chair;

	private void Awake()
	{
		ObjectivesController = FindObjectOfType<ObjectivesController>();
		FoodController = FindObjectOfType<FoodController>();
	}

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

	private void HidePlayer()
	{
		Base.gameObject.SetActive(false);
		Shirt.gameObject.SetActive(false);
		Shoes.gameObject.SetActive(false);
		Hair.gameObject.SetActive(false);
		Pants.gameObject.SetActive(false);
    }

	private void ShowPlayer()
    {
        Base.gameObject.SetActive(true);
        Shirt.gameObject.SetActive(true);
        Shoes.gameObject.SetActive(true);
        Hair.gameObject.SetActive(true);
        Pants.gameObject.SetActive(true);
    }

	int _playerDirectionFacing = 1; // -1 left : 1 right
	private void MovementUpdate()
	{
		// Can't move while interacting.
		if (IsInteracting )
		{
			return;
		}

		if (IsInventorying)
		{
			if (IsAutomoving)
			{
				IsAutomoving = false;
				AutomovePosition = null;
				_rigidBody.velocity = Vector3.zero;

				SetPlayerWalkingAnimation(false);
			}

			return;
		}

		if (IsAutomoving)
		{
			if (AutomovePosition == null) 
			{
				Debug.Log("No automove position is set. Stopping automove.");
				IsAutomoving = false;
			}
			else if (Math.Abs(transform.position.x - AutomovePosition.Value.x) < 0.2)
			{
				IsAutomoving = false;
				AutomovePosition = null;

				_rigidBody.velocity = Vector3.zero;

				if (_currentActiveInteractable != null)
				{
					Debug.Log($"Interacting with {_currentActiveInteractable.gameObject.name}.");
					_interactMenu = Instantiate(_currentActiveInteractable.InteractionMenu, SubTaskCenter.transform, false);
					_subTaskController = _interactMenu.GetComponent<InteractableSubTaskController>();
					_subTaskController.Player = this;

					IsInteracting = true;
					BackInteractionButton.SetActive(true);
				}

				SetPlayerWalkingAnimation(false);
			}

			return;
		}

		if (IsExploring) {
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

		var horizontalMoveDistance = deltaMove * GetPlayerMovementSpeed();
		_rigidBody.velocity = new Vector3(horizontalMoveDistance, 0f, 0f);
	}

	private float GetPlayerMovementSpeed()
	{
		var movementSpeedModifier = 1f;
		if (StatusController.HasBadStatus())
		{
			movementSpeedModifier -= 0.5f;
		}

		return MovementSpeed * movementSpeedModifier;
	}

	private bool _isTouchingRightWall = false;
	private bool _isTouchingLeftWall = false;
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

	public bool IsAutomoving = false;
	
	public Vector2? AutomovePosition = null;

	public void MovePlayerTo(Vector2 pos)
	{
		if (IsInteracting && !IsExploring) return;

		RecalculateAutomove(pos);
	}
	private void RecalculateAutomove(Vector2 pos)
	{
		if (Math.Abs(transform.position.x - pos.x) < 0.2) // if we are 0.1 close
		{
			IsAutomoving = false;
			AutomovePosition = null;

			StopMovement();

			if (_currentActiveInteractable != null)
			{
				Debug.Log($"Interacting with {_currentActiveInteractable.gameObject.name}.");
				_interactMenu = Instantiate(_currentActiveInteractable.InteractionMenu, SubTaskCenter.transform, false);
				_subTaskController = _interactMenu.GetComponent<InteractableSubTaskController>();
				_subTaskController.Player = this;

				IsInteracting = true;
				BackInteractionButton.SetActive(true);
			}
		}
        else
        {
			IsAutomoving = true;
			AutomovePosition = pos;
			var deltaMove = (transform.position.x < pos.x ? 1 : -1);
			_rigidBody.velocity = new Vector3(deltaMove * GetPlayerMovementSpeed(), 0, 0);

			SetPlayerWalkingAnimation(deltaMove != 0);

			// Check player facing direction
			if (deltaMove != 0 && deltaMove != _playerDirectionFacing)
			{
				_playerDirectionFacing = deltaMove;
				SetPlayerAnimationDirection(deltaMove);
			}
		}
    }

	public void SetPlayerPositionTo(Vector2 position)
	{
		transform.position = new Vector3(position.x, position.y, -0.5f);
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
		if (IsExploring)
		{
			return;
		}

		if (IsInteracting)
		{
			// No escape button to stop interacting
			//if (Input.GetKeyDown(KeyCode.Escape))
			//{
			//	StopInteracting();
			//}

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

			IsInteracting = true;

            BackInteractionButton.SetActive(true);

            StopMovement();
		}
	}

	public void Interact_OnGameTick(int timePassedPerTick)
	{
		Debug.Log($"Interact_OnGameTick({timePassedPerTick})");
		StatusController.Interact(_interaction, timePassedPerTick);

		if (_interaction.Name != "Disco" && _interaction.Name != "Library")
		{
			StopInteractingButton.SetActive(true);
		}

		if (_interaction.Name == "Work")
		{
			ObjectivesController.OnWorking(timePassedPerTick);
		}
		if (_interaction.HasMaximumTime && _interaction.CurrentMaximumTime <= 0)
		{
			Debug.Log("Interaction has reached maximum time. Stopping Interaction");
			if (_interaction.Name == "Disco" || _interaction.Name == "Library")
			{
				FindObjectOfType<ExplorationController>().ReturnHome();
				return;
			}

			StopInteracting();
		}
	}

	public bool CanInteract(InteractionScriptableObject interaction)
	{
		return StatusController.CanInteract(interaction);
	}

	private void StopMovement()
	{
		_rigidBody.velocity = Vector3.zero;
		SetPlayerWalkingAnimation(false);
	}

	private InteractionScriptableObject _interaction;
	private InteractionScriptableObject _exploreInteraction;
	public void StartInteracting(InteractionScriptableObject interaction, bool showInteractionButtons = true)
	{
		Debug.Log($"StartInteracting({interaction.name})");

		// Stop movement.
		StopMovement();

		if (interaction.IsFood && !FoodController.HasEatenToday)
		{
			FoodController.EatFood(interaction);
			return;
		}

		BackInteractionButton.SetActive(false);

		if (!showInteractionButtons)
		{
			StopInteractingButton.SetActive(false);
		}

		// KANNA
		if (interaction.Name == "Bed")
		{
			HidePlayer();
			BedAnim.enabled = true;
			bedplayerhead.SetActive(true);
			BedAnim.SetBool("IsSleeping", true);

		}
		if (interaction.IsFood)
		{
            _playerAnimators.ForEach(a => { a.SetBool("IsInteracting", true); });
        }
		if (interaction.IsBook)
		{
			playerBook.SetActive(true);
            _playerAnimators.ForEach(a => { a.SetBool("IsInteracting", true); });
            _playerAnimators.ForEach(a => { a.speed = 0; });
        }
        if (interaction.IsPeeCee)
		{
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.position = chair.transform.position;
            _playerAnimators.ForEach(a => { a.SetBool("IsSitting", true); });
        }

		if (interaction.Name == "Disco" || interaction.Name == "Library")
		{
			_exploreInteraction = interaction;
		}
		// This is the start of the interacting method.
		// Animation should be set to start here.

		_interaction = interaction;
		if (_interaction.HasMaximumTime)
		{
			_interaction.CurrentMaximumTime = _interaction.MaximumTime;
		}

		if (_interaction.IsFood)
		{
			ObjectivesController.OnFoodEaten(_interaction);
		}

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

		// KANNA
		// This is the start of the stop interacting method.
		// Animation should stop here.
		ShowPlayer();
        _playerAnimators.ForEach(a => { a.speed = 1; });
        playerBook.SetActive(false);
        BedAnim.enabled = false;
        bedplayerhead.SetActive(false);
        BedAnim.SetBool("IsSleeping", false);
        _playerAnimators.ForEach(a => { a.SetBool("IsInteracting", false); });
        _playerAnimators.ForEach(a => { a.SetBool("IsSitting", false); });

        TimeController.OnGameTick -= Interact_OnGameTick;
		StatusController.ResetInteraction();
		_interaction = null;
		IsInteracting = false;
		_subTaskController = null;
		Destroy(_interactMenu);
		_interactMenu = null;

		StopInteractingButton.SetActive(false);
		BackInteractionButton.SetActive(false);
	}

	public void BeginExplore(Vector2 pos)
	{
		IsInteracting = false;
		BackInteractionButton.SetActive(false);
		_interactMenu?.SetActive(false);
		MovePlayerTo(pos);
	}

	public void StartExploreAnimations()
	{
		if (_exploreInteraction.Name == "Disco") // the disco name should be the name of hte disco interaction in the name field
		{
			_playerAnimators.ForEach(a => { a.SetBool("IsInteracting", true); });
		}
		if (_exploreInteraction.Name == "Library") // same thing with disco, name field matches
		{
			playerBook.SetActive(true);
			_playerAnimators.ForEach(a => { a.SetBool("IsInteracting", true); });
			_playerAnimators.ForEach(a => { a.speed = 0; });
		}
	}
	public void StopExploreAnimations()
	{
		_exploreInteraction = null;
        playerBook.SetActive(false);
        _playerAnimators.ForEach(a => { a.SetBool("IsInteracting", false); });
		_playerAnimators.ForEach(a => { a.SetBool("IsSitting", false); });
        _playerAnimators.ForEach(a => { a.speed = 1; });
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
