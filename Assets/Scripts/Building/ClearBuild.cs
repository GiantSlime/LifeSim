using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClearBuild : MonoBehaviour
{
	public Sprite DefaultSprite;
	public Sprite HighlightedSprite;
	public Sprite ClickedSprite;

	private BuildSlot _parentBuildSlot;

	public void Start()
	{
		_parentBuildSlot = GetComponentInParent<BuildSlot>();
	}

	public void OnMouseDown()
	{
		_parentBuildSlot.ClearBuildSlot();
	}

	public void OnMouseUp()
	{

	}

	public void OnMouseEnter()
	{

	}

	private void OnMouseExit()
	{

	}
}
