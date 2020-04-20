using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class CameraController : MonoBehaviour
{
	public Transform target;

	public float maxDistanceDelta;
	public float maxRadiansDelta;
	public float maxMagnitudeDelta;

	public float perspectiveChangeTime;

	public float distortionAmount2D;
	public float distortionAmount3D;

	public Vector3 Perspective2DRotation;
	public Vector3 Perspective2DOffset;

	public Vector3 Perspective3DRotation;
	public Vector3 Perspective3DOffset;

	private Vector3 _offset = Vector3.zero;
	private Vector3 _rotation = Vector3.zero;
	private Camera _camera;

	[SerializeField]
	private Volume _pPVolume;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void LateUpdate()
	{	
		transform.eulerAngles = Vector3.RotateTowards(transform.eulerAngles, _rotation, maxRadiansDelta, maxMagnitudeDelta);
		transform.position = Vector3.MoveTowards(transform.position, target.position + _offset, maxDistanceDelta);
	}

	public void SetPerspective(Persepective persepective)
	{
		if (persepective == Persepective.Perspective3D)
		{
			_offset = Perspective3DOffset;
			_rotation = Perspective3DRotation;
			StartCoroutine(WaitSecondsAndSetOrtographicMode(perspectiveChangeTime, false));
			StartCoroutine(WaitSecondsAndSetDistortion(perspectiveChangeTime, distortionAmount3D));
		}
		else if (persepective == Persepective.Perspective2D)
		{
			_offset = Perspective2DOffset;
			_rotation = Perspective2DRotation;
			StartCoroutine(WaitSecondsAndSetOrtographicMode(perspectiveChangeTime, true));
			StartCoroutine(WaitSecondsAndSetDistortion(perspectiveChangeTime, distortionAmount2D));
		}
	}

	private IEnumerator WaitSecondsAndSetOrtographicMode(float seconds, bool value)
	{
		yield return new WaitForSeconds(seconds);
		_camera.orthographic = value;
	}

	private IEnumerator WaitSecondsAndSetDistortion(float seconds, float value)
	{
		yield return new WaitForSeconds(seconds);
		_pPVolume.profile.TryGet(out LensDistortion _lensDistortion);
		_lensDistortion.intensity.value = value;
	}
}
