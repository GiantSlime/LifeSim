using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
	public List<ItemScriptableObject> Inventory = new List<ItemScriptableObject>();

	public GameObject InventoryWindow;

	public Button DummyItemButton;

	private float _inventoryWindowWidth;
	private float _inventoryWindowHeight;

	public List<Button> InventoryItems = new List<Button>();

	public bool IsInventorying => _isInventoryOpened;

	private void Start()
	{
		var windowRect = InventoryWindow.GetComponent<RectTransform>().rect;
		_inventoryWindowWidth = windowRect.width * 2;
		_inventoryWindowHeight = windowRect.height * 2;
		Debug.Log($"Window width:{_inventoryWindowWidth}, height:{_inventoryWindowHeight}");
	}

	private bool _isInventoryOpened = false;
	public void ToggleInventoryWindow()
	{
		InventoryWindow.SetActive(!_isInventoryOpened);
		_isInventoryOpened = !_isInventoryOpened;

		if (_isInventoryOpened)
		{
			CreateInventoryItemButtons();
		}
		else
		{
			InventoryItems.ForEach(x => Destroy(x));
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
			DummyItemButton.image.sprite = item.Sprite;
			InventoryItems.Add(itemObject);
		}
	}

	public void AddItem(ItemScriptableObject item)
	{
		Inventory.Add(item);
	}
}
