using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private float movementInputDirection;
	
	private bool isFaceingRight = true;
	private bool isWalking;
	
	private Rigidbody2D rb;
	private Animator anim;
	
	
	[SerializeField] private float movementSpeed = 10.0f;
	[SerializeField] private float jumpForce = 16.0f;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
		CheckMovementDirection();
		UpdateAnimations();
    }
	
	private void FixedUpdate()
    {
        ApplyMovement();
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
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
}