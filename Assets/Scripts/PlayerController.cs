using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private float movementInputDirection;
	
	private int amountOfJumpsLeft;
	
	private bool isFaceingRight = true;
	private bool isWalking;
	private bool isGrounded;
	private bool canJump;
	
	private Rigidbody2D rb;
	private Animator anim;
	
	public int amountOfJumps = 1;
	
	
	[SerializeField] private float movementSpeed = 10.0f;
	[SerializeField] private float jumpForce = 16.0f;
	[SerializeField] private float groundCheckRadius;
	
	
	[SerializeField] private Transform groundCheck;
	
	[SerializeField] LayerMask whatIsGround;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
		CheckMovementDirection();
		UpdateAnimations();
		CheckIfCanJump();
    }
	
	private void FixedUpdate()
    {
        ApplyMovement();
		CheckSurrondings();
    }
	
	private void CheckSurrondings()
    {
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
	
	private void CheckIfCanJump()
	{
		if(( isGrounded && rb.velocity.y <= 0 ))
		{
			//canJump = true;
			amountOfJumpsLeft = amountOfJumps;
		}
		
		
		if ( amountOfJumpsLeft <= 0)
		{
			canJump = false;
		}
		else
		{
			canJump = true;
		}
	}
	
	private void CheckMovementDirection()
	{
		if(isFaceingRight && movementInputDirection < 0)
		{
			Flip();
		}
		else if (!isFaceingRight && movementInputDirection > 0)
		{
			Flip();
		}
		
		if(rb.velocity.x != 0)
		{
			isWalking = true;
		}
		else
		{
			isWalking = false;
		}
	}
	
	private void UpdateAnimations()
	{
		anim.SetBool("isWalking", isWalking);
	}
	
	private void CheckInput()
	{
		movementInputDirection = Input.GetAxisRaw("Horizontal");
		
		if(Input.GetButtonDown("Jump"))
		{
			Jump();
		}
	}
	
	private void Jump()
	{
		if (canJump)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			amountOfJumpsLeft--;
		}
	}
	
	private void ApplyMovement()
	{
		rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y );
	}
	
	private void Flip()
	{
		isFaceingRight = !isFaceingRight;
		transform.Rotate(0.0f, 180f, 0.0f);
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
	}
}