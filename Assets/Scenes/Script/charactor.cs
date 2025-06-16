using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	private bool isGameOver = false;
	public int HP = 3;
	private InputAction moveAction;
	private InputAction jumpAction;

	public Text hp_text;

	public GameObject Hit_prefab;

	public GameObject GameOver;

	public float speed = 3f;
	public float jumpForce = 5f;
	private bool isGrounded = true;

	private Rigidbody2D rb;

	private CapsuleCollider2D cc2d;

	private SpriteRenderer sr;

	Animator anim;



	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		cc2d = GetComponent<CapsuleCollider2D>();
		anim = GetComponent<Animator>();
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
		if (isGameOver)
		{
			rb.linearVelocity = new Vector2(0, -6);
			cc2d.enabled = false;
			sr.sortingOrder = 200;
			return;
		}
		
		
		Vector2 input = moveAction.ReadValue<Vector2>();


		if (rb.linearVelocity.y < 0.001f)
		{
			anim.SetBool("isJumping", false);
		}

		if (input.x != 0)
			{
				anim.SetBool("isIDLE", false);
				anim.SetBool("isRUN", true);

				GetComponent<SpriteRenderer>().flipX = input.x < 0;
			}
			else
			{
				anim.SetBool("isIDLE", true);
				anim.SetBool("isRUN", false);
			}
		// Rigidbody2D의 velocity로 이동
			rb.linearVelocity = new Vector2(input.x * speed, rb.linearVelocity.y);
		// transform.Translate(movement * speed * Time.deltaTime); // 기존 이동 코드 주석처리
	}


	// 연속 점프 방지
	private void OnJump(InputAction.CallbackContext context)
	{
		if (isGameOver) return;
		if (isGrounded)
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			isGrounded = false;
			anim.SetBool("isJumping", true);
			anim.SetBool("isFalling", false);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Tilemap")
			isGrounded = true;

		if (collision.gameObject.tag == "Obstacle")
		{
			HP--;
			hp_text.text = "HP: " + HP.ToString();

			GameObject HitPart = Instantiate(Hit_prefab, transform.position, Quaternion.identity);
			Destroy(HitPart, 1f);

			anim.SetTrigger("isHit");
		}

		if (collision.gameObject.tag == "ObstacleX2")
		{
			HP -= 2;
			hp_text.text = "HP: " + HP.ToString();
		}

		if (HP <= 0)
		{
			GameOver.SetActive(true);
			isGameOver = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		Debug.Log("STAY");
	}
	private void OisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		Debug.Log("EXIT");
	}


}
