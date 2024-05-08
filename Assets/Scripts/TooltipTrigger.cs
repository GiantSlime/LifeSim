using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Multiline]
	public string content;

	public string header;

	public void OnDestroy()
	{
		TooltipSystem.Hide();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("TootipTrigger:OnPointerEnter");
		TooltipSystem.Show(content, header);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("TootipTrigger:OnPointerExit");
		TooltipSystem.Hide();
	}

	public void OnMouseEnter()
	{
		Debug.Log("TootipTrigger:OnMouseEnter");
		TooltipSystem.Show(content, header);
	}

	private void OnMouseExit()
	{
		Debug.Log("TootipTrigger:OnMouseEnter");
		TooltipSystem.Hide();
	}
}
