using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds all game state data.
/// </summary>
public class GameController : MonoBehaviour
{
	private static GameController instance;
	public static GameController Instance;

	public List<ItemSale> ShopItems = new();

	private void Awake()
	{
		Debug.Log("GameController.Awake()");

		Instance = this;
	}
}
