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
	private bool isTouchingWall;
	private bool isWallSliding;
	private bool canJump;
	
	private Rigidbody2D rb;
	private Animator anim;
	
	public int amountOfJumps = 1;
	
	
	[SerializeField] private float movementSpeed = 10.0f;
	[SerializeField] private float jumpForce = 16.0f;
	[SerializeField] private float groundCheckRadius;
	[SerializeField] private float wallCheckDistance;
	[SerializeField] private float wallSlideSpeed;
	[SerializeField] private float movementForceInAir;
	[SerializeField] private float airDragMultiplier = 0.95f;
	[SerializeField] private float variableJumpHeightMultiplier = 0.5f;


	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform wallCheck;
	
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
		CheckIfWallSliding();
    }
	
	private void FixedUpdate()
    {
        ApplyMovement();
		CheckSurrondings();
    }
	
	private void CheckIfWallSliding()
	{
		if( isTouchingWall && !isGrounded && rb.velocity.y < 0)
		{
			isWallSliding = true;
		}
		else
		{
			isWallSliding = false;
		}
	}
	
	private void CheckSurrondings()
    {
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		
		isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
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
		anim.SetBool("isGrounded", isGrounded);
		anim.SetFloat("yVelocity", rb.velocity.y); 
		anim.SetBool("isWallSliding", isWallSliding);
		
	}
	
	private void CheckInput()
	{
		movementInputDirection = Input.GetAxisRaw("Horizontal");
		
		if(Input.GetButtonDown("Jump"))
		{
			Jump();
		}

		if (Input.GetButtonUp("Jump")) 
		{
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
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
		if(isGrounded)
		{
			rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y );
		}
		else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
		{
			Vector2 forceToAdd = new Vector2 (movementForceInAir * movementInputDirection, 0);
			rb.AddForce(forceToAdd);

			if(Mathf.Abs(rb.velocity.x) > movementSpeed) 
			{
				Debug.Log("limitar velocidad");
				rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);   
			}
		}
		else if (!isGrounded && !isWallSliding && movementInputDirection == 0) 
		{
			rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
		}
		
		
		if (isWallSliding)
		{
			if(rb.velocity.y < -wallSlideSpeed)
			{
				rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed); 
			}
		}
	}
	
	private void Flip()
	{
		if(!isWallSliding)
		{
			isFaceingRight = !isFaceingRight;
			transform.Rotate(0.0f, 180f, 0.0f);
		}
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		
		Gizmos.DrawLine(wallCheck.position, new Vector3 (wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
	}
}