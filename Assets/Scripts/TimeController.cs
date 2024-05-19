using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float TimeSpeedModifier = 1; // Time speed modifier. Time is calculated against TimeSpeed/TimeSpeedModifier
    public float TimeSpeed = 1; // How many real life seconds until a tick occurs
    public int TimePassedPerTick = 5; // How many in game minutes pass in a single tick

    public bool IsGameTimePaused = false;

    public DateTime time = DateTime.MinValue.AddHours(8); // Game starts at 8am
    public TextMeshProUGUI TimeText;

    public int Day = 1;
    public TextMeshProUGUI DayText;

	public event Action<int> OnGameTick;
    public event Action OnDayCycle;

    // Start is called before the first frame update
    public void Start()
    {
		OnGameTick += UpdateTime_OnGameTick;
        OnDayCycle += UpdateDay_OnDayCycle;
		DayText.text = $"Day {Day}";
		TimeText.text = time.ToShortTimeString();
	}

	private float ElapsedTime = 0;
    public void Update()
    {
        if (IsGameTimePaused)
            return;

        var timeCalculus = TimeSpeed / TimeSpeedModifier;

        ElapsedTime += Time.deltaTime;
        if (ElapsedTime < timeCalculus) 
        {
            return;
        }

		ElapsedTime -= timeCalculus;

        OnGameTick.Invoke(TimePassedPerTick);
	}

    public void UpdateTime_OnGameTick(int timePassedPerTick)
    {
        time = time.AddMinutes(timePassedPerTick);
        TimeText.text = time.ToShortTimeString();//$"{TimeSpan.Hours:D2}:{TimeSpan.Minutes:D2}";

		if (time.Day >= 2)
        {
            time = time.Subtract(TimeSpan.FromDays(1));
            OnDayCycle.Invoke();
        }
    }

    public void UpdateDay_OnDayCycle() 
    {
        Day += 1;
        DayText.text = $"Day {Day}";
    }

    public void SetTimeSpeedModifier(float speed)
    {
        TimeSpeedModifier = speed;
    }

	public void PauseGameTime()
	{
        IsGameTimePaused = true;
	}
	public void UnpauseGameTime()
	{
        IsGameTimePaused = false;
	}
}
