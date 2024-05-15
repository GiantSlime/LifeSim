using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public InventoryController InventoryController;

    private List<BuildSlot> _buildSlots = new List<BuildSlot>();
    private ItemScriptableObject _activeItem;


	private void Awake()
	{
        InventoryController = FindObjectOfType<InventoryController>();
        
        _buildSlots.Clear();
        InitializeBuildSlots();
	}

    private void InitializeBuildSlots()
    {
        var slots = GetComponentsInChildren<BuildSlot>();
        _buildSlots.AddRange(slots);
        _buildSlots.ForEach(slot =>
        {
            slot.BuildController = this;
            slot.OnBuildSlotClear += OnBuildSlotClear;
        });
    }

	/// <summary>
	/// This method is called by the InventoryManager when they click on a building to place, to allow the controller
	/// to start highlighting the correct building placement slots and temporarily storying the itemSO to be placed.
	/// </summary>
	/// <param name="item">The item that is currently set as active.</param>
	public void SetCurrentActiveItem(ItemScriptableObject item)
    {
        UnsetCurrentActiveItem();

        _activeItem = item;
        HighlightValidBuildSlotsForActiveItem();
    }

    public void UnsetCurrentActiveItem()
    {
        SetBuildSlotsToDefault();
        _activeItem = null;
    }

    /// <summary>
    /// This method is called by the build slots when they are clicked.
    /// </summary>
    /// <param name="buildSlot">The build slot that was clicked.</param>
    public void BuildSlotClicked(BuildSlot buildSlot)
    {
        Debug.Log("BuildSlotClicked:Enter");
		// no point doing anything if we don't have an active item
		if (_activeItem == null) return;

        Debug.Log("BuildSlotClicked: Active Item is not null");
		// can't place an item in an invalid placement slot
		if (!buildSlot.CanPlaceItemHere(_activeItem)) return;

        Debug.Log("Setting active item to valid build slot");
        SetActiveItemOnBuildSlot(buildSlot);

        InventoryController.ItemPlaced(_activeItem);

        Debug.Log("BuildSlotClicked:Exit");
    }

    private void SetActiveItemOnBuildSlot(BuildSlot buildSlot)
    {
		buildSlot.Item = _activeItem;
		buildSlot.ItemSpriteRenderer.sprite = _activeItem.Sprite;
		buildSlot.ClearBuild.gameObject.SetActive(true);
	}

    private void HighlightValidBuildSlotsForActiveItem()
    { 
        foreach (var slot in _buildSlots) {
            if (!slot.CanPlaceItemHere(_activeItem)) 
            {
                slot.SlotSpriteRenderer.sprite = slot.DisabledSprite;
                continue;
            }

            slot.SlotSpriteRenderer.sprite = slot.HighlightedSprite; // or highlighted?
        }
    }

    private void SetBuildSlotsToDefault()
    {
        _buildSlots.ForEach(slot => slot.SlotSpriteRenderer.sprite = slot.DefaultSprite);
    }

    private void OnBuildSlotClear(object sender, EventArgs e)
    {
		var slot = (BuildSlot)sender;
        if (slot == null)
        {
            Debug.Log("OnBuildSlotClear: sender is null");
        }

        Debug.Log("OnBuildSlotClear: Clearing build slot");
        var item = slot.Item;
        slot.Item = null;

        InventoryController.AddItem(item);

        slot.ClearBuild.gameObject.SetActive(false);
	}
}
