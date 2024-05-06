using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InteractionScriptableObject", order = 1)]
public class InteractionScriptableObject : ScriptableObject
{
	public string Name;

	[Range(-100, 100)]
	public int Energy;

	[Range(-100, 100)] 
	public int Fun;

	[Range(-100, 100)] 
	public int Hunger;

	public bool HasMaximumTime = false;
	public int MaximumTime = 0;

	[HideInInspector]
	public int CurrentMaximumTime = 0;

	public float MoneyCost = 0;
	public bool IsImmediateCost = false;
}

