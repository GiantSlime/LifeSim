using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public TimeController TimeController;
    public InventoryController InventoryController;
    public StatusController StatusController;

	public static List<ItemSale> ItemsList;

	[HideInInspector]
    public List<Button> ItemSaleButtonList = new();
    public List<ItemSale> ItemSaleList = new();

	public int MaximumItemsToLoad = 8;
    public int NumberOfItemsPerRow = 4;
    public int NumberOfRowsPerPage = 2;

    public GameObject ItemSaleButtonTemplate;


	private float _shopWindowWidth;
	private float _shopWindowHeight;


	// Start is called before the first frame update
	void Start()
    {
        LoadControllers();

		var windowRect = GetComponent<RectTransform>().rect;
		_shopWindowWidth = windowRect.width * 2;
		_shopWindowHeight = windowRect.height * 2;
	}

	private void OnEnable()
	{
        LoadControllers();
        OnLoad();
	}

	private void OnDisable()
	{
		ItemSaleButtonList.ForEach(button => Destroy(button.gameObject));
        ItemSaleList.Clear();
	}

    private void LoadControllers()
    {
        if (TimeController == null)
		    TimeController = FindFirstObjectByType<TimeController>();
        if (InventoryController == null)
		    InventoryController = FindFirstObjectByType<InventoryController>();
		if (StatusController == null)
            StatusController = FindFirstObjectByType<StatusController>();
	}

    public void OnLoad()
    {
        var itemsToSell = GetRandomItems();
        CreateButtons(itemsToSell);
    }

    public IList<ItemSale> GetRandomItems()
    {
        // Create copy of sale item list
        ItemSale[] itemsArray = new ItemSale[ItemsList.Count];
        ItemsList.CopyTo(itemsArray);
        var itemsList = itemsArray.ToList();

        // Remove from copy, all items that player already has
        foreach (var item in InventoryController.Inventory)
        {
            var itemIndex = itemsList.FindIndex(i => i.Item.Name == item.Name);
            if (itemIndex < 0) continue;

            itemsList.RemoveAt(itemIndex);
        }
		
        // Randomly select using the remaining items up to MaximumItemsToLoad
		Random.InitState(TimeController.Day);
		var itemsToLoad = new List<ItemSale>();
        for (int i = 0; i < Mathf.Min(MaximumItemsToLoad, itemsList.Count); i++)
        {
            var itemIndex = Random.Range(0, itemsList.Count);
            var item = itemsList[itemIndex];
            itemsList.RemoveAt(itemIndex);

            itemsToLoad.Add(item);
        }

        return itemsToLoad;
    }

    public void CreateButtons(IList<ItemSale> itemsToSell)
    {
        for (var i = 0; i < itemsToSell.Count; i++)
        {
            var itemSale = itemsToSell[i];

            int column = i % NumberOfItemsPerRow;
            int row = (NumberOfRowsPerPage - 1) - (i / NumberOfItemsPerRow); // Taking away so we got top down instead of bottom up

            var x = (column + 1) * (_shopWindowWidth / (NumberOfItemsPerRow + 1));
            var y = (row + 1) * (_shopWindowHeight / (NumberOfRowsPerPage + 1));

            // This is done because gameobject position is centered.
            x -= _shopWindowWidth / 2;
            y -= _shopWindowHeight / 2;

            Debug.Log($"Creating item number: {i}, at x:{x},y:{y}, column:{column},row:{row}, a:{_shopWindowWidth},b:{_shopWindowHeight / NumberOfItemsPerRow}");

            var itemGameObject = Instantiate(ItemSaleButtonTemplate, this.transform.position + new Vector3(x, y, 0), Quaternion.identity, this.transform);
            // set cost text
            var tmp = itemGameObject.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = $"${itemSale.Cost}";
            // set button event to purchase
            var btn = itemGameObject.GetComponent<Button>();
            btn.image.sprite = itemSale.Item.Sprite;
            btn.onClick.AddListener(delegate { TryPurchaseItem(itemSale.Item.Name); });
            btn.interactable = StatusController.CanAffordCost(itemSale.Cost);
            // set tooltip to item name
            var tltp = itemGameObject.GetComponent<TooltipTrigger>();
            tltp.content = itemSale.Item.Name;
            ItemSaleButtonList.Add(btn);
            ItemSaleList.Add(itemSale);
        }
    }

    public void TryPurchaseItem(string name)
    {
        var index = ItemSaleList.FindIndex(x => x.Item.Name == name);
        if (index < 0) 
        {
            Debug.Log($"can't find item(name={name}) in itemsale list. ignoring click");
            return;
        }

        var itemSale = ItemSaleList[index];

        ItemSaleButtonList[index].interactable = false;

        ItemsList.Remove(itemSale);

        StatusController.SubtractCost(itemSale.Cost);

        InventoryController.AddItem(itemSale.Item);
    }
}
