using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	public int HP = 3;
	private InputAction moveAction;
	private InputAction jumpAction;

	public Text hp_text;


	public float speed = 3f;
	public float jumpForce = 5f;
	private bool isGrounded = true;

	private Rigidbody2D rb;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		moveAction = new InputAction(type: InputActionType.Value);

		moveAction.AddCompositeBinding("2DVector")
		.With("Left", "<Keyboard>/a")
		.With("Right", "<Keyboard>/d");

		moveAction.AddCompositeBinding("2DVector")
		.With("Left", "<Keyboard>/leftArrow")
		.With("Right", "<Keyboard>/rightArrow");

		moveAction.Enable();

		jumpAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
		jumpAction.performed += OnJump;
		jumpAction.Enable();

	}

	private void OnDisable()
	{
		moveAction.Disable();
		jumpAction.Disable();
	}

	void Start()
	{
		hp_text.text = "HP : "+HP.ToString();
	}


	// Update is called once per frame
	void Update()
	{
		Vector2 input = moveAction.ReadValue<Vector2>();

		if (input.x != 0)
		{
			GetComponent<SpriteRenderer>().flipX = input.x < 0;
		}
		// Rigidbody2D의 velocity로 이동
		rb.linearVelocity = new Vector2(input.x * speed, rb.linearVelocity.y);
		// transform.Translate(movement * speed * Time.deltaTime); // 기존 이동 코드 주석처리
	}


	// 연속 점프 방지
	private void OnJump(InputAction.CallbackContext context)
	{
		if (isGrounded)
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			isGrounded = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Tilemap")
		isGrounded = true;

		if (collision.gameObject.tag == "SpikeHead")
		{
			HP--;
			hp_text.text = "HP: "+HP.ToString();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "SpikeHead")
		Debug.Log("STAY");
	}
	private void OisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "SpikeHead")
		Debug.Log("EXIT");
	}


}
