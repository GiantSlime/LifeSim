using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemScriptableObject", order = 2)]
public class ItemScriptableObject : ScriptableObject
{
	public string Name;

	public Sprite Sprite;

	public PlacementType[] PlacementTypes;
}
