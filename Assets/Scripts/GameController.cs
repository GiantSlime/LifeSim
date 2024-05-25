using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class holds all game state data.
/// </summary>
public class GameController : MonoBehaviour
{
	private static GameController instance;
	public static GameController Instance;

	public List<ItemSale> ShopItems = new();
	public int NumberOfItemsPerCategoryToLoad = 1;

	private List<ItemSale> _carCategoryItemSaleList = new();
	private List<ItemSale> _floralCategoryItemSaleList = new();
	private List<ItemSale> _artisticCategoryItemSaleList = new();
	private List<ItemSale> _gamerCategoryItemSaleList = new();
	private List<ItemSale> _retroCategoryItemSaleList = new();

	[HideInInspector]
	public List<ItemSale> TodaysItems = new();

	[HideInInspector]
	public TimeController TimeController;
	[HideInInspector]
	public QuestionController QuestionController;

	public int NumberOfDaysForGame = 3;

	private void Awake()
	{
		Debug.Log("GameController.Awake()");

		Instance = this;

		TimeController = FindObjectOfType<TimeController>();
		QuestionController = FindObjectOfType<QuestionController>();
	}

	private void Start()
	{
		InitializeShopCategories();
		ResetDailyShopItems();
		TimeController.OnDayCycle += OnDayCycle;
	}

	private void OnDayCycle(int day)
	{
		ResetDailyShopItems();
		if (day > NumberOfDaysForGame)
		{
			TimeController.PauseGameTime();
			QuestionController.StartQuestions();
		}
	}

	// Separates all items into their individual category lists
	private void InitializeShopCategories()
	{
		ItemSale[] itemsArray = new ItemSale[Instance.ShopItems.Count];
		Instance.ShopItems.CopyTo(itemsArray);
		var itemsList = itemsArray.ToList();

		foreach (var item in itemsList)
		{
			item.Item.IsItemPurchased = false;

			switch (item.Item.ItemCategory)
			{
				case ItemCategory.Car:
					_carCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Floral:
					_floralCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Artistic:
					_artisticCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Gamer:
					_gamerCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Retro:
					_retroCategoryItemSaleList.Add(item);
					break;
			}
		}
	}

	private void ResetDailyShopItems()
	{
		foreach (var item in TodaysItems) 
		{
			if (item.Item.IsItemPurchased)
			{
				continue;
			}

			switch (item.Item.ItemCategory)
			{
				case ItemCategory.Car:
					_carCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Floral:
					_floralCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Artistic:
					_artisticCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Gamer:
					_gamerCategoryItemSaleList.Add(item);
					break;
				case ItemCategory.Retro:
					_retroCategoryItemSaleList.Add(item);
					break;
			}
		}

		TodaysItems.Clear();

		// Add items from each category
		TodaysItems.AddRange(GetRandomItemFromCategoryList(_carCategoryItemSaleList));
		TodaysItems.AddRange(GetRandomItemFromCategoryList(_floralCategoryItemSaleList));
		TodaysItems.AddRange(GetRandomItemFromCategoryList(_artisticCategoryItemSaleList));
		TodaysItems.AddRange(GetRandomItemFromCategoryList(_gamerCategoryItemSaleList));
		TodaysItems.AddRange(GetRandomItemFromCategoryList(_retroCategoryItemSaleList));
	}

	/// <summary>
	/// Returns a random list of items equal to the number of items per category to load
	/// </summary>
	/// <param name="itemsList"></param>
	/// <returns></returns>
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
}
