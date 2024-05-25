using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AnswerType
{
	StronglyAgree,
	Agree,
	Disagree,
	StronglyDisagree
}

public enum AnswerValue
{
	Positive,
	Negative
}

[Serializable]
public class Question
{
	public string QuestionText;
	public AnswerValue AnswerValue;
}

public class QuestionController : MonoBehaviour
{
	public GameObject QuestionsWindow;
	public TextMeshProUGUI QuestionsText;

	public List<Question> Questions = new();

	public Button StronglyAgree;
	public Button Agree;
	public Button Disagree;
	public Button StronglyDisagree;

	[HideInInspector]
	public ObjectivesController ObjectivesController;

	private int _currentQuestionIndex = 0;

	private void Awake()
	{
		ObjectivesController = FindObjectOfType<ObjectivesController>();
	}

	public void Start()
	{
		StronglyAgree.onClick.AddListener(delegate { StronglyAgreeClick(); });
		Agree.onClick.AddListener(delegate {  AgreeClick(); });
		Disagree.onClick.AddListener(delegate {  DisagreeClick(); });
		StronglyDisagree.onClick.AddListener(delegate { StronglyDisagreeClick(); });
	}

	public void StartQuestions()
	{
		_currentQuestionIndex = 0;
		if (Questions.Count == 0) 
		{
			Debug.Log("There were no questions to ask. Ending game.");
			Application.Quit();
			return;
		}

		QuestionsWindow.SetActive(true);
		QuestionsText.text = Questions[_currentQuestionIndex].QuestionText;
	}

	private void SelectAnswer(AnswerType answer)
	{
		ObjectivesController.OnQuestionAnswered(Questions[_currentQuestionIndex], answer);
		SetNextQuestion();
	}

	private void SetNextQuestion()
	{
		_currentQuestionIndex++;
		if (_currentQuestionIndex >= Questions.Count)
		{
			// Maybe change to end scene
			Application.Quit();
			return;
		}

		QuestionsText.text = Questions[_currentQuestionIndex].QuestionText;
	}

	private void StronglyAgreeClick()
	{
		SelectAnswer(AnswerType.StronglyAgree);
	}
	private void AgreeClick()
	{
		SelectAnswer(AnswerType.Agree);
	}
	private void DisagreeClick()
	{
		SelectAnswer(AnswerType.Disagree);
	}
	private void StronglyDisagreeClick()
	{
		SelectAnswer(AnswerType.StronglyDisagree);
	}
}
