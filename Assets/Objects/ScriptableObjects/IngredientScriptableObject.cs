using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType 
{
	Normal,
	Weird
}

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 3)]
public class IngredientScriptableObject : ScriptableObject
{
	public string Name;
	public Sprite Image;
	public IngredientType IngredientType;
}
