using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MovementSpeed = 1.0f;

	private List<GameObject> _interactableGameObjects;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		MovementUpdate();
		InteractablesUpdate();
	}

	private void MovementUpdate()
	{
		float deltaMove = 0f;

		if (Input.GetKey(KeyCode.A))
		{
			deltaMove -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			deltaMove += 1;
		}

		var horizontalMoveDistance = deltaMove * MovementSpeed * Time.deltaTime;
		transform.position += new Vector3(horizontalMoveDistance, 0f, 0f);
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
			throw new System.Exception($"InteractableBase not found in object={closestInteractable} even though it is in list of interactable gameobjects");
		}

		if (interactableBase == _currentActiveInteractable)
			return;

		// turn off old, turn on new
		_currentActiveInteractable?.SetActiveInteractable(false);
		interactableBase.SetActiveInteractable(true);
		_currentActiveInteractable = interactableBase;
	}

	/// <summary>
	/// Gets the absolute distance from a gameobject to this object.
	/// </summary>
	/// <param name="gameObject">The object to get the distance from</param>
	/// <returns>The absolute distance</returns>
	private float GetDistanceFromObject(GameObject gameObject)
	{
		return System.Math.Abs(this.transform.position.x - gameObject.transform.position.x);
	}
}
