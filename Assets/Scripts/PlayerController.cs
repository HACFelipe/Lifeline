using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	public float horizontalVelociy;
	public float runVelocity;
	public float soarVelocity;
	public float jumpVelocity;
	public float fallFactor;
	public float forceLandingFactor;
	public float lowJumpFactor;
	public float groundCheckDistance;
	public float forwardCheckDistance;
	public bool isGrounded;

	private Persepective _currentPerspective;

	public GameObject explosion;

	private float _currentHorizontalVelocity;
	private float _vAxis;
	private float _hAxis;

	public AudioClip jump;
	public AudioClip hit;
	public AudioClip death;

	private AudioSource _audioSource;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
		Time.timeScale = 1.0f;
	}

	private void Update()
	{
		isGrounded = IsGrounded();

		_vAxis = Input.GetAxisRaw("Vertical");
		_hAxis = _currentPerspective == Persepective.Perspective2D ? 0.0f : Input.GetAxis("Horizontal");
		_currentHorizontalVelocity = _currentPerspective == Persepective.Perspective2D ? 0.0f : horizontalVelociy;

		if (isGrounded)
		{
			if (rb.velocity.y < -1.0f)
			{
				_audioSource.clip = hit;
				_audioSource.Play();
			}

			if (Input.GetButtonDown("Jump"))
			{
				_audioSource.clip = jump;
				_audioSource.Play();
				rb.velocity += Vector3.up * jumpVelocity;
			}
		}

		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector3.up * Physics.gravity.y * (fallFactor - 1) * Time.deltaTime;
			if (_vAxis > 0.0f)
			{
				rb.velocity += Vector3.up * soarVelocity * _vAxis;
			}
		}
		else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpFactor - 1) * Time.deltaTime;
		}

		if (_vAxis < 0)
		{
			rb.velocity += Vector3.up * Physics.gravity.y * (forceLandingFactor - 1) * Time.deltaTime;
		}

		rb.velocity += Vector3.right * runVelocity * Time.deltaTime;
		rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Mathf.Clamp(_currentHorizontalVelocity * -_hAxis, -2.0f, 2.0f));

		if (transform.position.y <= -20.0f)
		{
			Die();
		}
	}

	private void FixedUpdate()
	{
		if (ForwardCollision())
		{
			Die();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Spike"))
		{
			Die();
		}
	}

	private bool IsGrounded()
	{
		Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.blue);
		return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
	}

	private bool ForwardCollision()
	{
		Debug.DrawRay(transform.position, Vector3.right * forwardCheckDistance, Color.red);
		if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hitInfo, forwardCheckDistance))
		{
			return !hitInfo.collider.CompareTag("PerspectiveChanger");
		}
		return false;
	}

	public void SetPerspective(Persepective perspective)
	{
		_currentPerspective = perspective;
	}

	public void Die()
	{
		_audioSource.clip = death;
		_audioSource.Play();
		StartCoroutine(GameOver());
		Instantiate(explosion, transform.position, Quaternion.identity);
		enabled = false;
	}

	public IEnumerator GameOver()
	{
		yield return new WaitForSeconds(1.2f);
		SceneManager.LoadScene("Game Over", LoadSceneMode.Additive);
	}
}
