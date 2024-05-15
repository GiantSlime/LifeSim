using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.VisualScripting;

public class StatusController : MonoBehaviour
{
	public Slider EnergyBarSlider;
	public Slider FunBarSlider;
	public Slider HungerBarSlider;

	public TextMeshProUGUI moneyText;

	private float _money;
	public float Money {
		get => _money;
		set 
		{
			_money = value;
			moneyText.text = $"${(int)_money}";
		}
	}

	[Range(0, 100)]
	public float EnergyLevel = 100;
	[Range(0, 100)]
	public float FunLevel = 100;
	[Range(0, 100)]
	public float HungerLevel = 100;

	public void AcceptCost(float cost)
	{
		Money += cost;
	}
	public void SubtractCost(float cost) 
	{
		Money -= cost;
	}
	public bool CanAffordCost(float cost)
	{
		return Money >= cost;
	}

	public void Interact(InteractionScriptableObject interaction, int time)
	{
		Debug.Log($"Interact(interaction:{interaction.name}, time:{time})");

		// Gets either time passed or time remaining, whichever is smaller
		if (interaction.HasMaximumTime)
		{
			if (time > interaction.CurrentMaximumTime)
			{
				time = interaction.CurrentMaximumTime;
				interaction.CurrentMaximumTime = 0;
			}
			else
			{
				interaction.CurrentMaximumTime -= time;
			}

			Debug.Log($"Interaction remaining time: {interaction.CurrentMaximumTime}");
		}

		var interactionTimeSpent = time / 60f;

		EnergyLevel = Math.Max(Math.Min(EnergyLevel + interaction.Energy * interactionTimeSpent, 100), 0);
		FunLevel = Math.Max(Math.Min(FunLevel + interaction.Fun * interactionTimeSpent, 100), 0);
		HungerLevel = Math.Max(Math.Min(HungerLevel + interaction.Hunger * interactionTimeSpent, 100), 0);

		EnergyBarSlider.value = EnergyLevel;
		FunBarSlider.value = FunLevel;
		HungerBarSlider.value = HungerLevel;

		_timeSpentInteracting += time;
		if (_timeSpentInteracting < 60) return;

		if (interaction.Money > 0)
		{
			Money += interaction.Money;
		}

		_timeSpentInteracting = 0;
	}

	private int _timeSpentInteracting = 0;
	public void ResetInteraction()
	{
		_timeSpentInteracting = 0;
	}

	public bool HasBadStatus()
	{
		if (EnergyLevel == 0 || FunLevel == 0 || HungerLevel == 0)
			return true;

		return false;
	}

	public bool CanInteract(InteractionScriptableObject interaction)
	{
		if (interaction.Money > 0) return true;
		else 
			return Money >= -1 * interaction.Money;
	}
}
