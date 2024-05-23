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
    public ObjectivesController ObjectivesController;

	[HideInInspector]
    public List<Button> ItemSaleButtonList = new();
    private List<ItemSale> _itemSaleList = new();

	private int MaximumItemsToLoad = 5;
    private int NumberOfItemsPerRow = 5;
    private int NumberOfRowsPerPage = 1;
    public int NumberOfItemsPerCategoryToLoad = 1;

    public GameObject ItemSaleButtonTemplate;

	private float _shopWindowWidth;
	private float _shopWindowHeight;

	private void Awake()
	{
		ObjectivesController = FindObjectOfType<ObjectivesController>();
	}

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
        _itemSaleList.Clear();
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
        ItemSale[] itemsArray = new ItemSale[GameController.Instance.ShopItems.Count];
		GameController.Instance.ShopItems.CopyTo(itemsArray);
        var itemsList = itemsArray.ToList();

        // Instead of randomly selecting, we now want one random item from each category to show up
        List<ItemSale> carCategory = new();
        List<ItemSale> floralCategory = new();
        List<ItemSale> artisticCategory = new();
        List<ItemSale> gamerCategory = new();
        List<ItemSale> retroCategory = new();

        foreach (var item in itemsList)
        {
            switch (item.Item.ItemCategory)
            {
                case ItemCategory.Car:
                    carCategory.Add(item);
                    break;
                case ItemCategory.Floral:
                    floralCategory.Add(item);
                    break;
                case ItemCategory.Artistic:
                    artisticCategory.Add(item);
                    break;
                case ItemCategory.Gamer: 
                    gamerCategory.Add(item);
                    break;
                case ItemCategory.Retro:
                    retroCategory.Add(item);
                    break;
            }
        }

		// Randomly select using the remaining items up to MaximumItemsToLoad
		var itemsToLoad = new List<ItemSale>();
        itemsToLoad.AddRange(GetRandomItemFromCategoryList(carCategory));
        itemsToLoad.AddRange(GetRandomItemFromCategoryList(floralCategory));
        itemsToLoad.AddRange(GetRandomItemFromCategoryList(artisticCategory));
        itemsToLoad.AddRange(GetRandomItemFromCategoryList(gamerCategory));
        itemsToLoad.AddRange(GetRandomItemFromCategoryList(retroCategory));

        return itemsToLoad;
    }

    private List<ItemSale> GetRandomItemFromCategoryList(List<ItemSale> itemsList)
    {
        var returnList = new List<ItemSale>();
		Random.InitState(TimeController.Day);
        for (var i = 0; i < Mathf.Min(NumberOfItemsPerCategoryToLoad, itemsList.Count); i++)
        {
			var itemIndex = Random.Range(0, itemsList.Count);
			var item = itemsList[itemIndex];
			itemsList.RemoveAt(itemIndex);

			returnList.Add(item);
		}

        return returnList;
	}

	public void CreateButtons(IList<ItemSale> itemsToSell)
    {
        for (var i = 0; i < itemsToSell.Count; i++)
        {
            var itemSale = itemsToSell[i];

            int column = i % NumberOfItemsPerRow;
            int row = (NumberOfRowsPerPage - 1) - (i / NumberOfItemsPerRow); // Taking away so we got top down instead of bottom up

			var windowRect = GetComponent<RectTransform>().rect;
			_shopWindowWidth = windowRect.width * 2;
			_shopWindowHeight = windowRect.height * 2;

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
            _itemSaleList.Add(itemSale);
        }
    }

    public void TryPurchaseItem(string name)
    {
        var index = _itemSaleList.FindIndex(x => x.Item.Name == name);
        if (index < 0) 
        {
            Debug.Log($"can't find item(name={name}) in itemsale list. ignoring click");
            return;
        }

        var itemSale = _itemSaleList[index];

        ItemSaleButtonList[index].interactable = false;

		GameController.Instance.ShopItems.Remove(itemSale);

        StatusController.SubtractCost(itemSale.Cost);

        InventoryController.AddItem(itemSale.Item);

        ObjectivesController.OnItemBought();
    }
}
