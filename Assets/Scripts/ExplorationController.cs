using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ExplorationController : MonoBehaviour
{
	public PlayerController PlayerController;

	public bool HasExplorationBegun = false;
	public bool IsExploring = false;
	public bool IsInteracting = false;
	public bool HasReachedExplorePosition = false;
	public Vector2? ExplorePosition = null;
	public bool IsPlayerMoving = false;
	public ExploreType CurrentExploreType = ExploreType.None;

	public GameObject DiscoBackground;
	public GameObject DiscoPlayerLocation;
	public GameObject LibraryBackground;
	public GameObject LibraryPlayerLocation;

	public Transform OutsideHomePosition;
	public Transform OutsideExplorePosition;
	public Transform InsideHomePositon;

	public Button EndExplorationButtion;

	public bool IsReturning = false;
	public bool HasReturnedHome = false;

	public GameObject BottomFloorCover;

	public ObjectivesController ObjectivesController;
	public TimeController TimeController;

	private bool _HasExploredToday = false;

	public enum ExploreType
	{
		None,
		Disco,
		Library
	}

	private bool moveStarted = false;

	public void Awake()
	{
		ObjectivesController = FindObjectOfType<ObjectivesController>();
		TimeController = FindObjectOfType<TimeController>();
	}

	public void Start()
	{
		TimeController.OnDayCycle += OnNewDay;
	}

	public void Update()
	{
		if (!IsExploring || IsInteracting) return; // Don't do anything unless we're exploring

        if (moveStarted)
        {
			moveStarted = PlayerController.IsAutomoving;
			return;
        }

        if (PlayerController.IsAutomoving) return; // Wait until player has reached
												   // no automoving means player has reached location.

		if (IsReturning)
		{
			if (!HasReturnedHome)
			{
				PlayerController.SetPlayerPositionTo(OutsideHomePosition.position);
				Vector2 homeLocation = InsideHomePositon.transform.position;
				PlayerController.MovePlayerTo(homeLocation);
				HasReturnedHome = true;
				moveStarted = true;
			}
			else
			{
				BottomFloorCover.SetActive(true);
				ResetExploration();
				PlayerController.GetComponent<BoxCollider2D>().enabled = true;
				PlayerController.IsExploring = false;
			}
		}
		else // i.e. starting
		{
			if (!HasReachedExplorePosition)
			{
				PlayerController.SetPlayerPositionTo(OutsideExplorePosition.position);
				var exploreLocation = GetCurrentExploreLocation();
				if (exploreLocation == null) return; // Reset, something went wrong, we don't have an explore location.
				PlayerController.MovePlayerTo(exploreLocation.Value);
				HasReachedExplorePosition = true;
				moveStarted = true;
			}
			else
			{
				// Set start explore animation
				IsInteracting = true;
				EndExplorationButtion.gameObject.SetActive(true);
			}
		}
	}

	public void ResetExploration()
	{
		HasExplorationBegun = false;
		HasExplorationBegun = false;
		IsExploring = false;
		IsInteracting = false;
		HasReachedExplorePosition = false;
		IsPlayerMoving = false;
		CurrentExploreType = ExploreType.None;
		IsReturning = false;
		HasReturnedHome = false;
		moveStarted = false;
	}

	public Vector2? GetCurrentExploreLocation()
	{
		switch (CurrentExploreType)
		{
			case ExploreType.None:
				return null;
			case ExploreType.Disco:
				return DiscoPlayerLocation.transform.position;
			case ExploreType.Library:
				return LibraryPlayerLocation.transform.position;
		}

		return null;
	}

	public void StartExploring(ExploreType exploreType)
	{
		if (_HasExploredToday)
		{
			Debug.Log("StartExploring:Player has already explored today.");
			return;
		}

		PlayerController.IsExploring = true;
		PlayerController.GetComponent<BoxCollider2D>().enabled = false;
		PlayerController.MovePlayerTo(OutsideHomePosition.position);
		LoadExploreScreen(exploreType);
		CurrentExploreType = exploreType;
		moveStarted = true;
		IsExploring = true;
		ObjectivesController.OnGoneOutside();

		_HasExploredToday = true;
	}

	public void LoadExploreScreen(ExploreType exploreType)
	{
		BottomFloorCover.SetActive(false);
		switch (exploreType)
		{
			case ExploreType.Disco:
				DiscoBackground.SetActive(true);
				break;
			case ExploreType.Library:
				LibraryBackground.SetActive(true);
				break;
		}
	}

	public void ReturnHome()
	{
		IsInteracting = false;
		IsReturning = true;
		PlayerController.MovePlayerTo(OutsideExplorePosition.position);
		EndExplorationButtion.gameObject.SetActive(false);
	}

	private void OnNewDay()
	{
		_HasExploredToday = false;
	}
}
