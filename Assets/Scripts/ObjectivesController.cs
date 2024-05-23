using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ObjectivesController : MonoBehaviour
{
    public TextMeshProUGUI TouchGrassObjective;
    public TextMeshProUGUI HungryObjective;
    public TextMeshProUGUI WorkObjective;
    public TextMeshProUGUI BuyAnItemObjective;

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
            SetObjectiveAsCompleted(WorkObjective);
        }
    }
    public void OnGoneOutside()
    {
        Debug.Log("OnGoneOutside: Objective Complete.");
        HasGoneOutside = true;
        SetObjectiveAsCompleted(TouchGrassObjective);
    }
    public void OnFoodEaten()
    {
        Debug.Log("OnFoodEaten: Objective Complete.");
        HasEatenFood = true;
        SetObjectiveAsCompleted(HungryObjective);
    }
    public void OnItemBought()
    {
        Debug.Log("OnItemBought: Objective Complete.");
        HasBoughtItem = true;
        SetObjectiveAsCompleted(BuyAnItemObjective);
    }

    private void OnNewDay()
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

    private void SetObjectiveAsCompleted(TextMeshProUGUI objectiveText)
    {
		ShowObjectivesPanel();
		//objectiveText.color = ColorUtility.TryParseHtmlString("#808080", out var color) ? Color.black : color;
		objectiveText.fontStyle = FontStyles.Strikethrough;
        StartCoroutine("HideObjectivesCoroutine");
    }

    private void ResetObjectives()
	{
		TouchGrassObjective.color = Color.white;
		TouchGrassObjective.fontStyle = FontStyles.Normal;
		HungryObjective.color = Color.white;
		HungryObjective.fontStyle = FontStyles.Normal;
		WorkObjective.color = Color.white;
		WorkObjective.fontStyle = FontStyles.Normal;
		BuyAnItemObjective.color = Color.white;
		BuyAnItemObjective.fontStyle = FontStyles.Normal;

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
