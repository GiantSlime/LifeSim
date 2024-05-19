using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
	public List<ItemScriptableObject> Inventory = new List<ItemScriptableObject>();

	public GameObject InventoryWindow;
	public Button InventoryButton;

	public Button DummyItemButton;

	private float _inventoryWindowWidth;
	private float _inventoryWindowHeight;

	public List<Button> InventoryItems = new List<Button>();

	public bool IsInventorying => _isInventoryOpened;
	public BuildController BuildController;

	public PlayerController PlayerController;
	public TimeController TimeController;

	private void Start()
	{
		var windowRect = InventoryWindow.GetComponent<RectTransform>().rect;
		_inventoryWindowWidth = windowRect.width * 2;
		_inventoryWindowHeight = windowRect.height * 2;
		Debug.Log($"Window width:{_inventoryWindowWidth}, height:{_inventoryWindowHeight}");

		PlayerController = FindObjectOfType<PlayerController>();
		TimeController = FindObjectOfType<TimeController>();
	}

	private bool _isInventoryOpened = false;
	public void ToggleInventoryWindow()
	{
		if (PlayerController.IsInteracting) 
		{
			Debug.Log("Player is interacting; unable to open inventory");
			return;
		}

		_isInventoryOpened = !_isInventoryOpened;
		InventoryWindow.SetActive(_isInventoryOpened);

		if (_isInventoryOpened)
		{
			ReloadInventory();
		}

		BuildController.SetBuildMode(_isInventoryOpened);

		if (_isInventoryOpened)
		{
			TimeController.PauseGameTime();
		}
		else
		{
			TimeController.UnpauseGameTime();
		}
	}

	public const int NumberOfItemsPerRow = 4;
	public const int NumberOfRowsPerPage = 2;
	public void CreateInventoryItemButtons()
	{
		Debug.Log("CreateInventoryItemButtons");
		for (var i = 0; i < Inventory.Count; i++)
		{
			var item = Inventory[i];
			int column = i % NumberOfItemsPerRow;
			int row = (NumberOfRowsPerPage - 1) - (i / NumberOfItemsPerRow); // Taking away so we got top down instead of bottom up

			var x = (column + 1) * (_inventoryWindowWidth / (NumberOfItemsPerRow + 1));
			var y = (row + 1) * (_inventoryWindowHeight / (NumberOfRowsPerPage + 1));

			// This is done because gameobject position is centered.
			x -= _inventoryWindowWidth / 2;
			y -= _inventoryWindowHeight / 2;

			Debug.Log($"Creating item number: {i}, at x:{x},y:{y}, column:{column},row:{row}, a:{_inventoryWindowWidth},b:{_inventoryWindowWidth / NumberOfItemsPerRow}");

			var itemObject = Instantiate(DummyItemButton, InventoryWindow.transform.position + new Vector3(x, y, 0), Quaternion.identity, InventoryWindow.transform);
			itemObject.image.sprite = item.Sprite;
			itemObject.onClick.AddListener(() => ItemClicked(item));
			InventoryItems.Add(itemObject);
		}
	}

	public void ReloadInventory()
	{
		Debug.Log("ReloadInventory");
		InventoryItems.ForEach(x => Destroy(x.gameObject));
		InventoryItems.Clear();
		CreateInventoryItemButtons();
	}

	private ItemScriptableObject _activeItem;
	public void ItemClicked(ItemScriptableObject item)
	{
		if (item == null)
		{
			Debug.Log("ItemClicked: Null item was passed as an argument.");
			return;
		}
		if (_activeItem == item)
		{
			Debug.Log("ItemClicked: Active item is same as item clicked. Unsetting item as active");
			BuildController.UnsetCurrentActiveItem();
			_activeItem = null;
		}
		else
		{
			Debug.Log("ItemClicked: Active item is different than item clicked. Changing active item.");
			_activeItem = item;
			BuildController.SetCurrentActiveItem(_activeItem);
		}
	}

	public void AddItem(ItemScriptableObject item)
	{
		Inventory.Add(item);
		ReloadInventory();
	}

	public void ItemPlaced(ItemScriptableObject item) 
	{
		Debug.Log("ItemPlaced: Removing item from inventory");
		Inventory.Remove(item);
		_activeItem = null;
		ReloadInventory();
	}
}
