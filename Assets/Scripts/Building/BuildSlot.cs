using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSlot : MonoBehaviour
{
    // This is the allow list of the types of placements that can be put on this BuildSlot.
    public List<PlacementType> PlacementTypes = new();

    public ItemScriptableObject Item;

    public Sprite DefaultSprite;                // The default look when a building can be placed here
    public Sprite HighlightedSprite;            // When hovered over this slot
    public Sprite DisabledSprite;               // probably show this if the active clicked build can't be placed here
    public Sprite SelectableSprite;             // When selecting an item, this is the sprite for slots that can be selected

    public SpriteRenderer BorderSpriteRenderer;
    public SpriteRenderer SlotSpriteRenderer;   // Should be set by the editor
    public SpriteRenderer ItemSpriteRenderer;   // Should be set by the editor

    public ClearBuild ClearBuild;               // Manager of the clear button on the build slot

    public BuildController BuildController;

	public event EventHandler OnBuildSlotClear;

	public void Start()
	{
        BorderSpriteRenderer = GetComponent<SpriteRenderer>();
        SlotSpriteRenderer.sprite = DefaultSprite;
	}

	private void OnMouseEnter()
	{
        Debug.Log("OnMouseEnter");
	}

	private void OnMouseExit()
	{
        Debug.Log("OnMouseExit");
	}

	public void OnMouseDown()
	{
		if (Item == null)
        {
            Debug.Log("BuildSlot:OnMouseDown: Item is null.");
        }

        Debug.Log("BuildSlot:OnMouseDown");
        BuildController.BuildSlotClicked(this);
	}

	/// <summary>
	/// Validates whether or not an item can be placed here.
	/// </summary>
	public bool CanPlaceItemHere(ItemScriptableObject item)
    {
        foreach (PlacementType placementType in item.PlacementTypes)
		{
			if (PlacementTypes.Contains(placementType))
			{
				return true;
			}
		}

        return false;
	}

    public void ClearBuildSlot()
    {
        if (Item == null)
        {
            Debug.Log("ClearBuildSlot: No item exists in build slot");
            return;
        }

        OnBuildSlotClear.Invoke(this, null);
    }

    public void OnItemPlaced()
    {
        ItemSpriteRenderer.enabled = true;
        ClearBuild.Renderer.enabled = true;
    }

    public void OnItemRemoved()
    {
        ItemSpriteRenderer.enabled = false;
        ClearBuild.Renderer.enabled = false;
	}
}
