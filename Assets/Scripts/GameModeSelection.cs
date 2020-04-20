using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameDifficulty
{
	Easy,
	Medium,
	Expert
}

public class GameModeSelection : MonoBehaviour
{

	public Transform easyPos;
	public Transform mediumPos;
	public Transform expertPos;

	public GameDifficulty chosenDifficulty = GameDifficulty.Easy;

	public AudioClip select;

	private AudioSource _audioSource;

	private static GameModeSelection _instance;

	public static GameModeSelection Instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		_instance = this;
		GameObject[] objs = GameObject.FindGameObjectsWithTag("GameMode");

		if (objs.Length > 1)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioSource.clip = select;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
		{
			if (chosenDifficulty == GameDifficulty.Easy)
			{
				chosenDifficulty = GameDifficulty.Medium;
				transform.position = mediumPos.position;
			}
			else if (chosenDifficulty == GameDifficulty.Medium)
			{
				chosenDifficulty = GameDifficulty.Expert;
				transform.position = expertPos.position;
			}
			_audioSource.Play();
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			if (chosenDifficulty == GameDifficulty.Expert)
			{
				chosenDifficulty = GameDifficulty.Medium;
				transform.position = mediumPos.position;
			}
			else if (chosenDifficulty == GameDifficulty.Medium)
			{
				chosenDifficulty = GameDifficulty.Easy;
				transform.position = easyPos.position;
			}
			_audioSource.Play();
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			SceneManager.LoadScene("Game");
		}
	}
}
