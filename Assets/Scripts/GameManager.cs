using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Persepective
{
	Perspective2D,
	Perspective3D
}

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	[SerializeField]
	private PlayerController _playerController;
	[SerializeField]
	private CameraController _cameraController;

	[SerializeField]
	private Persepective _currentPerspective;

	[Tooltip("time scale amount per minute")]
	public float timeScaleIncreaseRate;

	public TextMeshProUGUI scoreText;
	public float score = 0.0f;

	public TextMeshProUGUI bpmText;
	public float bpm = 0.0f;

	[SerializeField]
	private List<GameObject> _platforms2D;
	[SerializeField]
	private List<GameObject> _platforms3D;
	private List<GameObject> _generatedPlatforms;
	[SerializeField]
	private GameObject _lastGenerated;

	[Tooltip("in seconds")]
	[SerializeField]
	private float levelCleanupTime;
	[SerializeField]
	private float minGenerationDistance;
	[SerializeField]
	private float minCleanDistance;

	public static GameManager Instance
	{
		get { return _instance; }
	}

	public Persepective CurrentPerspective
	{
		get { return _currentPerspective; }
		set
		{
			SetPerspective(value);
			_currentPerspective = value;
		}
	}

	private void Awake()
	{
		_instance = this;
		CurrentPerspective = Persepective.Perspective2D;
		_generatedPlatforms = new List<GameObject>();
	}

	private void Start()
	{
		SetDifficulty(GameModeSelection.Instance.chosenDifficulty);
		StartCoroutine(CleanUpPlatforms());
	}

	private void Update()
	{
		bpm = _playerController.runVelocity * Time.timeScale;
		bpmText.text = "BPM : " + bpm.ToString("F0");
		scoreText.text = "SCORE: " + score.ToString("F2");
		if (_playerController.isActiveAndEnabled)
		{
			score += Time.deltaTime;
		}
		if (_playerController.transform.position.x >= _lastGenerated.transform.position.x - minGenerationDistance)
		{
			GenerateNextPlatform();
		}

		Time.timeScale += (timeScaleIncreaseRate / 60.0f) * Time.deltaTime;
	}

	private void SetPerspective(Persepective perspective)
	{
		_cameraController.SetPerspective(perspective);
		_playerController.SetPerspective(perspective);
	}

	public void GenerateNextPlatform()
	{
		GameObject nextPlatform = ChooseRandomPlatform(CurrentPerspective);
		float size = nextPlatform.GetComponent<Platform>().size;
		GameObject _generated = Instantiate(nextPlatform, _lastGenerated.transform.position + Vector3.right * size, Quaternion.identity);
		_generatedPlatforms.Add(_generated);
		_lastGenerated = _generated;
	}

	public GameObject ChooseRandomPlatform(Persepective perspective)
	{
		if (perspective == Persepective.Perspective2D)
		{
			return _platforms2D[Random.Range(0, _platforms2D.Count)];
		}
		else
		{
			return _platforms3D[Random.Range(0, _platforms3D.Count)];
		}
	}

	private IEnumerator CleanUpPlatforms()
	{
		while (true)
		{
			Debug.Log("Cleaning up platforms!");
			List<GameObject> _toDelete = new List<GameObject>();
			yield return new WaitForSeconds(levelCleanupTime);
			foreach (GameObject platform in _generatedPlatforms)
			{
				float size = platform.GetComponent<Platform>().size;
				if (platform.transform.position.x <= _playerController.transform.position.x - minCleanDistance - size)
				{
					Destroy(platform);
					_toDelete.Add(platform);
				}
			}
			foreach (GameObject platform in _toDelete)
			{
				_generatedPlatforms.Remove(platform);
			}
		}
	}

	public void SetDifficulty(GameDifficulty difficulty)
	{
		if (difficulty == GameDifficulty.Easy)
		{
			timeScaleIncreaseRate = 0.5f;
		}
		else if (difficulty == GameDifficulty.Medium)
		{
			timeScaleIncreaseRate = 1.0f;
		}
		else
		{
			timeScaleIncreaseRate = 2.0f;
		}
	}
}
