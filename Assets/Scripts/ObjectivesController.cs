using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesController : MonoBehaviour
{
    public TextMeshProUGUI TouchGrassObjective;
    public TextMeshProUGUI HungryObjective;
    public TextMeshProUGUI WorkObjective;
    public TextMeshProUGUI BuyAnItemObjective;

    public Image TouchGrassCheckBox;
    public Image HungryCheckBox;
    public Image WorkCheckBox;
    public Image BuyAnItemCheckBox;

    public PlayerController playerController;
    public TimeController timeController;
    public StatusController statusController;

    public float TimeSpentWorkingToday = 0;
    public float RequiredTimeToWorkForObjective = 120;
    public bool HasWorkedEnough = false;
    public bool HasGoneOutside = false;
    public bool HasEatenFood = false;
    public bool HasBoughtItem = false;

    public List<Objectives> ObjectivesCompletionList = new();

    private RectTransform RectTransform;
    private Vector3 OriginalPosition;

    public Sprite OpenCheckbox;
    public Sprite CheckedCheckbox;

    private int _actionScore = 6;
    private int _questionsScore = 0;
    private List<string> _events = new();

	private void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
        timeController = FindObjectOfType<TimeController>();
        statusController = FindObjectOfType<StatusController>();

        timeController.OnDayCycle += OnNewDay;

        RectTransform = GetComponent<RectTransform>();
        OriginalPosition = RectTransform.position;

		ResetObjectives();

        HideObjectivesPanel();
	}

	public void OnApplicationQuit()
	{
        // save data
        var dataDirectory = Path.Join(Application.dataPath, "Data");

		if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }

        var saveName = $"{Guid.NewGuid()}.txt";

        var saveData = _events.ToLineSeparatedString();
        // Calculate Items Value
        var itemsValue = CalculateItemsValue();
        _actionScore += itemsValue;
        saveData += $"\nitems value added = {itemsValue}\n";
        saveData += $"\nAction Score: {_actionScore}";
		saveData += $"\nQuestions Score: {_questionsScore}";

		// Get save data

		File.WriteAllText(Path.Join(dataDirectory, saveName), saveData);
	}

    private int CalculateItemsValue()
    {
        var totalNumberOfItemsBought = 0;
        foreach (var item in _itemsBought)
        {
            totalNumberOfItemsBought += item.Value;
        }

        if (totalNumberOfItemsBought == 0) 
        {
            return 0;
        }

        if (totalNumberOfItemsBought == 3) // 3 items bought
        {
            if (_itemsBought.Count == 1) // all same category
			{
                return 2; 
            }

            if (_itemsBought.Count == 2) // 2 different categories (i.e. 2 of one category and 1 of another category
            {
                return 1;
            }

            if (_itemsBought.Count == 3) // 3 different categories
            {
                return -2;
            }
        }

        if (totalNumberOfItemsBought == 1)
        {
            return -1; // kanna how many points for only buying 1
        }

        if (totalNumberOfItemsBought == 2) // 2 items bought
        {
            if (_itemsBought.Count == 2) // 2 different categories
            {
                return -2; // kanna how many points
            }

            if (_itemsBought.Count == 1) // 2 of the same category
            {
                return 2; // kanna how many points
            }
        }

        return 0;
    }

	public void OnWorking(float time)
    {
        Debug.Log($"OnWorking: ObjectiveTicked, timePassed={time}, totalTimeWorked={TimeSpentWorkingToday += time}");
        if (HasWorkedEnough)
        {
            return;
        }

        if (TimeSpentWorkingToday > RequiredTimeToWorkForObjective)
        {
            Debug.Log("OnWorking: Objective Complete.");
			HasWorkedEnough = true;
            SetObjectiveAsCompleted(WorkObjective, WorkCheckBox);
        }
    }
    public void OnGoneOutside(ExplorationController.ExploreType location)
    {
        Debug.Log("OnGoneOutside: Objective Complete.");
        HasGoneOutside = true;
        SetObjectiveAsCompleted(TouchGrassObjective, TouchGrassCheckBox);
        var score = 0;
        if (location == ExplorationController.ExploreType.Disco) 
        {
			score--;
        }
        else if (location == ExplorationController.ExploreType.Library)
		{
			score++;
        }
        AddEvent($"Exploration: location={location}, value={score}");
        _actionScore += score;
	}
	public void OnFoodEaten(InteractionScriptableObject food)
    {
        Debug.Log("OnFoodEaten: Objective Complete.");
        HasEatenFood = true;
        SetObjectiveAsCompleted(HungryObjective, HungryCheckBox);
        AddEvent($"FoodEaten: Food={food.Name}");
    }
    private Dictionary<string, int> _itemsBought = new();
    public void OnItemBought(ItemSale itemSale)
    {
        Debug.Log("OnItemBought: Objective Complete.");
        HasBoughtItem = true;
        SetObjectiveAsCompleted(BuyAnItemObjective, BuyAnItemCheckBox);
        AddEvent($"ItemBought: Item={itemSale.Item.Name}, Category={itemSale.Item.ItemCategory}");
        if (_itemsBought.TryGetValue(itemSale.Item.ItemCategory.ToString(), out var _))
        {
            _itemsBought[itemSale.Item.ItemCategory.ToString()] += 1;
        }
        else
        {
			_itemsBought[itemSale.Item.ItemCategory.ToString()] = 1;
		}
	}
    public void OnIngredientPicked(IngredientScriptableObject ingredient)
    {
        var score = 0;
		if (ingredient.IngredientType == IngredientType.Normal)
		{
			score++;
		}
		else if (ingredient.IngredientType == IngredientType.Weird)
		{
			score--;
		}
		AddEvent($"IngredientPicked: ingredient={ingredient.Name}, value={score}");
        _actionScore += score;
	}
	public void OnQuestionAnswered(Question question, AnswerType answer)
    {
        int scoreDiff = 0;
        switch (answer)
		{
			case AnswerType.StronglyAgree:
				scoreDiff += 2 * (question.AnswerValue == AnswerValue.Positive ? 1 : -1);
				break;
			case AnswerType.Agree:
				scoreDiff += 1 * (question.AnswerValue == AnswerValue.Positive ? 1 : -1);
				break;
			case AnswerType.Disagree:
				scoreDiff += -1 * (question.AnswerValue == AnswerValue.Positive ? 1 : -1);
				break;
			case AnswerType.StronglyDisagree:
				scoreDiff += -2 * (question.AnswerValue == AnswerValue.Positive ? 1 : -1);
				break;
		}

        AddEvent($"QuestionAnswered: Question={question.QuestionText}, answer={answer}, value={scoreDiff}");
        _questionsScore += scoreDiff;
    }
	private void AddEvent(string eventName)
    {
        _events.Add(eventName);
    }

    private void OnNewDay(int day)
    {
        var todaysObjectives = new Objectives
        {
            TimeSpentWorking = TimeSpentWorkingToday,
            HasGoneOutside = HasGoneOutside,
            HasEaten = HasEatenFood
        };
        ObjectivesCompletionList.Add(todaysObjectives);
        ResetObjectives();
    }

    private void SetObjectiveAsCompleted(TextMeshProUGUI objectiveText, Image objectiveCheckbox)
    {
		ShowObjectivesPanel();
        //objectiveText.color = ColorUtility.TryParseHtmlString("#808080", out var color) ? Color.black : color;
        //objectiveText.fontStyle = FontStyles.Strikethrough;
        objectiveCheckbox.sprite = CheckedCheckbox;
        StartCoroutine("HideObjectivesCoroutine");
    }

    private void ResetObjectives()
	{
		TouchGrassObjective.color = Color.white;
		TouchGrassObjective.fontStyle = FontStyles.Normal;
        TouchGrassCheckBox.sprite = OpenCheckbox;
		HungryObjective.color = Color.white;
		HungryObjective.fontStyle = FontStyles.Normal;
        HungryCheckBox.sprite = OpenCheckbox;
		WorkObjective.color = Color.white;
		WorkObjective.fontStyle = FontStyles.Normal;
        WorkCheckBox.sprite = OpenCheckbox;
		BuyAnItemObjective.color = Color.white;
		BuyAnItemObjective.fontStyle = FontStyles.Normal;
        BuyAnItemCheckBox.sprite = OpenCheckbox;

        TimeSpentWorkingToday = 0;
        HasWorkedEnough = false;
        HasGoneOutside = false;
        HasEatenFood = false;
	}

    private bool _isObjectivesToggled = false;
    private bool _isToggling = false;
    public void ToggleObjectivesPanel()
    {
        if (_isToggling) return;

        _isObjectivesToggled = !_isObjectivesToggled;
        if (_isObjectivesToggled)
        {
            ShowObjectivesPanel();
            StartCoroutine("HideObjectivesCoroutine");
		}
		else
        {
            HideObjectivesPanel();
        }
    }

    private void ShowObjectivesPanel()
    {   
        // KANNA
        // The following code moves the position of the objectives rect
        var rect = RectTransform.position;
        rect.x = 0;
        RectTransform.position = OriginalPosition;
    }
    private void HideObjectivesPanel()
    {
		var rect = RectTransform.position;
		rect.x = -RectTransform.rect.width*2 - 500;
		RectTransform.position = rect;
	}

    private IEnumerator HideObjectivesCoroutine()
    {
        yield return new WaitForSeconds(2);

        HideObjectivesPanel();
    }
}
