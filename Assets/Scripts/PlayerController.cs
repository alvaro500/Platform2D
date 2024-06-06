using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private float movementInputDirection;
	private Rigidbody2D rb;
	private bool isFaceingRight = true;
	
	[SerializeField] private float movementSpeed = 10.0f;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
		CheckMovementDirection();
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
	}
	
	private void CheckInput()
	{
		movementInputDirection = Input.GetAxisRaw("Horizontal");
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