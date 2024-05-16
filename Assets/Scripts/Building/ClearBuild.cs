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

	private SpriteRenderer _renderer;

	public void Start()
	{
		_parentBuildSlot = GetComponentInParent<BuildSlot>();
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void OnEnable()
	{
		_renderer.sprite = DefaultSprite;
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
		_renderer.sprite = HighlightedSprite;
	}

	private void OnMouseExit()
	{
		_renderer.sprite = DefaultSprite;
	}
}
