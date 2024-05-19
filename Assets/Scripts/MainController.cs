using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
/// This holds character data from scene to scene.
/// </summary>
public class MainController : MonoBehaviour
{
    public PlayerController playerController;

	public static MainController Instance;

	private static readonly Dictionary<string, object> DataDictionary = new Dictionary<string, object>();

	private void Awake()
	{
		Debug.Log("MainController.Awake()");
		if (Instance != null)
		{
			Destroy(gameObject);
			Instance.RecalculatePlayerController();
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void RecalculatePlayerController()
	{
		playerController = FindObjectOfType<PlayerController>();
		playerController.LoadCharacterisation(DataDictionary);
	}

	public void SavePlayerData(Color baseColor, Color shirtColor, Color shoeColor, Color hairColor, 
							   Color pantsColor, RuntimeAnimatorController hairAC, RuntimeAnimatorController pantsAC, string name)
	{
		DataDictionary["BaseColor"] = baseColor;
		DataDictionary["ShirtColor"] = shirtColor;
		DataDictionary["ShoeColor"] = shoeColor;
		DataDictionary["HairColor"] = hairColor;
		DataDictionary["PantsColor"] = pantsColor;
		DataDictionary["HairAC"] = hairAC;
		DataDictionary["PantsAC"] = pantsAC;
		DataDictionary["Name"] = name;
	}
}
