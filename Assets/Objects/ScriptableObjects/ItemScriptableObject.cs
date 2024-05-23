using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 2)]
public class ItemScriptableObject : ScriptableObject
{
	public string Name;

	public Sprite Sprite;

	public PlacementType[] PlacementTypes;

	public ItemCategory ItemCategory;

	[HideInInspector]
	public bool IsItemPurchased = false;
}
