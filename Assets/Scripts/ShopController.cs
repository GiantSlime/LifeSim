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

    private int NumberOfItemsPerRow = 5;
    private int NumberOfRowsPerPage = 1;

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
        var itemsToSell = GameController.Instance.TodaysItems;
        CreateButtons(itemsToSell);
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

			var x = (column + 1) * (_shopWindowWidth / (Mathf.Min(NumberOfItemsPerRow, itemsToSell.Count) + 1));
            var y = (row + 1) * (_shopWindowHeight / (NumberOfRowsPerPage + 1));

            // This is done because gameobject position is centered.
            x -= _shopWindowWidth / 2;
            y -= _shopWindowHeight / 2;

            Debug.Log($"Creating item number: {i}, at x:{x},y:{y}, column:{column},row:{row}, a:{_shopWindowWidth},b:{_shopWindowHeight / NumberOfItemsPerRow}");

            var itemGameObject = Instantiate(ItemSaleButtonTemplate, this.transform.position + new Vector3(x, y, 0) * 1.5f, Quaternion.identity, this.transform);
            // set cost text
            var tmp = itemGameObject.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = $"${itemSale.Cost}";
            // set button event to purchase
            var btn = itemGameObject.GetComponent<Button>();
			btn.image.sprite = itemSale.Item.Sprite;
            btn.onClick.AddListener(delegate { TryPurchaseItem(itemSale.Item.Name); });
            btn.interactable = !itemSale.Item.IsItemPurchased && StatusController.CanAffordCost(itemSale.Cost);

            // set tooltip to item name
            var tltp = itemGameObject.GetComponent<TooltipTrigger>();
            tltp.header = itemSale.Item.Name;
            tltp.content = itemSale.Item.ItemCategory.ToString();

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

        itemSale.Item.IsItemPurchased = true;
		//GameController.Instance.TodaysItems.Remove(itemSale);

        StatusController.SubtractCost(itemSale.Cost);

        InventoryController.AddItem(itemSale.Item);

        ObjectivesController.OnItemBought();
    }
}
