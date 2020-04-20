using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePerspective : MonoBehaviour
{
	private AudioSource _audioSource;

	public Persepective desiredPerspective;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.Instance.CurrentPerspective = desiredPerspective;
			_audioSource.Play();
		}
	}
}
