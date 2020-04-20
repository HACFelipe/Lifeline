using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetScore : MonoBehaviour
{
	public TextMeshProUGUI text;

	private void Start()
	{
		text.text = GameManager.Instance.score.ToString("F2");
	}
}
