using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodController : MonoBehaviour
{
    private bool _hasEatenToday = false;
	public bool HasEatenToday => _hasEatenToday;

	[HideInInspector]
    public TimeController TimeController;
	[HideInInspector]
	public ObjectivesController ObjectivesController;

	private InteractionScriptableObject _foodToEat;

	[HideInInspector]
	public PlayerController PlayerController;

	public GameObject SpecialRandomIngredientMenu;
	public GameObject IngredientsPanel;

	public List<IngredientScriptableObject> Ingredients = new();
	public GameObject IngredientButtonTemplate;

	private void Awake()
	{
		TimeController = FindObjectOfType<TimeController>();
		ObjectivesController = FindObjectOfType<ObjectivesController>();
		PlayerController = FindObjectOfType<PlayerController>();
	}

	private void Start()
	{
		TimeController.OnDayCycle += OnDayCycle;
		CreateIngredientButtons();
	}

	private void OnDayCycle()
	{
		_hasEatenToday = false;
	}

	private void CreateIngredientButtons()
	{
		for (var i = 0; i < Ingredients.Count; i++)
		{
			var ingredient = Ingredients[i];

			int column = i % 4;
			int row = i / 4; // Taking away so we got top down instead of bottom up

			var windowRect = IngredientsPanel.GetComponent<RectTransform>().rect;
			var _shopWindowWidth = windowRect.width * 2;
			var _shopWindowHeight = windowRect.height * 2;

			var x = (column + 1) * (_shopWindowWidth / (Mathf.Min(4, Ingredients.Count) + 1));
			var y = (row + 1) * (_shopWindowHeight / 3);

			// This is done because gameobject position is centered.
			x -= _shopWindowWidth / 2;
			y -= _shopWindowHeight / 2;

			Debug.Log($"Creating item number: {i}, at x:{x},y:{y}, column:{column},row:{row}, a:{_shopWindowWidth},b:{_shopWindowHeight / 4}");

			var itemGameObject = Instantiate(IngredientButtonTemplate, IngredientsPanel.transform.position + new Vector3(x, y, 0), Quaternion.identity, IngredientsPanel.transform);
			// set button event to purchase
			var btn = itemGameObject.GetComponent<Button>();
			btn.image.sprite = ingredient.Image;
			btn.onClick.AddListener(delegate { FoodOnClick(ingredient.Name); });

			// set tooltip to item name
			var tltp = itemGameObject.GetComponent<TooltipTrigger>();
			tltp.header = ingredient.Name;
		}
	}

	public void EatFood(InteractionScriptableObject interaction)
	{
		if (!interaction.IsFood)
		{
			Debug.LogError("Eating interaction that isn't food!");
			return;
		}
		else if (_hasEatenToday)
		{
			Debug.Log("Already ate food today.");
			return;
		}

		_hasEatenToday = true;
		_foodToEat = interaction;

		SpecialRandomIngredientMenu.SetActive(true);
	}

	private void FoodOnClick(string name)
	{
		if (_foodToEat == null)
		{
			Debug.Log("FoodOnClick:Food to eat wasn't set.");
			return;
		}

		// Objectives hidden data +/-
		var ingredient = Ingredients.FindIndex(x => x.Name == name);
		// ObjectivesController.AddIngredientSelection(ingredient);

		SpecialRandomIngredientMenu.SetActive(false);

		PlayerController.StartInteracting(_foodToEat);

		_foodToEat = null;
	}
}
