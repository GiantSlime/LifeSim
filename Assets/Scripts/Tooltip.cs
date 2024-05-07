using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
	public TextMeshProUGUI headerField;
	public TextMeshProUGUI contentField;

	public LayoutElement layoutElement;

	private RectTransform rectTransform;

	public void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		var position = Input.mousePosition;
		var normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
		var pivot = CalculatePivot(normalizedPosition);
		rectTransform.pivot = pivot;
		transform.position = position;
	}

	private Vector2 CalculatePivot(Vector2 normalizedPosition)
	{
		var pivotTopLeft = new Vector2(-0.05f, 1.05f);
		var pivotTopRight = new Vector2(1.05f, 1.05f);
		var pivotBottomLeft = new Vector2(-0.05f, -0.05f);
		var pivotBottomRight = new Vector2(1.05f, -0.05f);

		if (normalizedPosition.x < 0.5f && normalizedPosition.y >= 0.5f)
		{
			return pivotTopLeft;
		}
		else if (normalizedPosition.x > 0.5f && normalizedPosition.y >= 0.5f)
		{
			return pivotTopRight;
		}
		else if (normalizedPosition.x <= 0.5f && normalizedPosition.y < 0.5f)
		{
			return pivotBottomLeft;
		}
		else
		{
			return pivotBottomRight;
		}
	}

	public void SetText(string content, string header = "")
	{
		headerField.gameObject.SetActive(!string.IsNullOrEmpty(header));
		headerField.text = content;

		contentField.text = content;
	}
}
