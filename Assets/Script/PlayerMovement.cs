using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    // MOVEMENT VARIABLES
    private float horizontalInput;
    private float speed = 8.0f;
    private float jumpingPower = 12.0f;
    private bool isFacingRight = true;

    // COYOTE TIME VARIABLES
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // JUMP BUFFERING VARIABLES
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // WALL SLIDING VARIABLES
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    // WALL JUMPING VARIABLES
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 12f);

    // SERIALIZEFIELDS
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    void Update()
    {
        
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (IsGrounded()) 
        {
        
            coyoteTimeCounter = coyoteTime;

        }
        else 
        {
        
            coyoteTimeCounter -= Time.deltaTime;

        }
        
        if (Input.GetKeyDown(KeyCode.W)) 
        {
        
            jumpBufferCounter = jumpBufferTime;
        
        }
        else
        {
        
            jumpBufferCounter -= Time.deltaTime;

        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) 
        {
        
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        
        }

        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
        
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;

        }

        WallSlide();
        WallJump();

        if(!isWallJumping) 
        {
        
            Flip();

        }

        

    }

    
    private void FixedUpdate() 
    {
    
        if(!isWallJumping) 
        {
        
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        
        }


    }


    private bool IsGrounded() 
    {
    
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }


    private bool IsWalled() 
    {
    
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }


    private void WallSlide() 
    {
    
        if (IsWalled() && !IsGrounded() && horizontalInput != 0f) 
        {
        
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else 
        {
        
            isWallSliding = false;

        }

    }


    private void WallJump() 
    {
    
        if (isWallSliding) 
        {
        
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));

        }
        else 
        {
        
            wallJumpingCounter -= Time.deltaTime;

        }

        if (Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f) 
        {
            
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection) 
            {
            
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        
        }
    
    }

    private void StopWallJumping() 
    {
    
        isWallJumping = false;

    }


    private void Flip() 
    {
    
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
        
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    
    }
}
