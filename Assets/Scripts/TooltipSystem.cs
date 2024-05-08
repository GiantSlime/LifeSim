using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
	private static TooltipSystem instance;

	public Tooltip tooltip;

	public void Awake()
	{
		instance = this;
	}

	public static void Show(string content, string header = "")
	{
		if (instance.tooltip.IsDestroyed()) return;

		instance?.tooltip?.SetText(content, header);
		instance?.tooltip?.gameObject?.SetActive(true);
	}

	public static void Hide()
	{
		if (instance.tooltip.IsDestroyed()) return;

		instance?.tooltip?.gameObject?.SetActive(false);
	}
}
