using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFilter : MonoBehaviour
{
	RectTransform rect;

	private void Start()
	{
		rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		Vector2 position = Vector2.up * 5.0f * (Mathf.Sin(Time.time) / 2.0f);
		rect.anchoredPosition += position.normalized / 2.0f;
	}
}
