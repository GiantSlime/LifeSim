using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusController : MonoBehaviour
{
	public Slider EnergyBarSlider;
	public Slider FunBarSlider;
	public Slider HungerBarSlider;

	[Range(0, 100)]
	public float EnergyLevel = 100;
	[Range(0, 100)]
	public float FunLevel = 100;
	[Range(0, 100)]
	public float HungerLevel = 100;

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
	}

	public bool HasBadStatus()
	{
		if (EnergyLevel == 0 || FunLevel == 0 || HungerLevel == 0)
			return true;

		return false;
	}
}
