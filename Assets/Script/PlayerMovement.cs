using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    // MOVEMENT VARIABLES (BASIC VARIABLES NEEDED FOR MOVEMENT)
    private float horizontalInput;
    private float speed = 8.0f;
    private float jumpPower = 12.0f;
    private bool isFacingRight = true;

    // COYOTE TIME & JUMP BUFFERING VARIABLES (INVISIBLE TRICKS THAT HELP MAKE THE PLATFORMING MORE ENJOYABLE)
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // WALL SLIDING & WALL JUMPING VARIABLES (FIRST MECHANIC // WILL BE USED IN PLATFORMING FOR VARIETY)
    private bool isWallSliding;
    private float wallSlideSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    private Vector2 wallJumpPower = new Vector2(8f, 14f);

    // DASHING VARIABLES (SECOND MECHANIC // SAME AS ABOVE)
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    void Update()
    {
        if (isDashing)
        {
            return;
        }
    
        // DETECTS HORIZONTAL INPUT
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // CHECKS TO SEE IF PLAYER IS GROUNDED; COYOTE TIME AND JUMP BUFFERING ALLOW FOR MORE LENIENT AND RESPONSIVE PLATFORMING
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

        // IF PLAYER IS GROUNDED, ALLOWS THEM TO JUMP. IF COYOTE TIME COUNTER AND JUMP BUFFER COUNTER ARE GREATER THEN 0, ALLOWS PLAYER TO JUMP EVEN WHILE OFF THE GROUND
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);       
        }


        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {       
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

        // CALLS WALL SLIDING AND WALL JUMPING FUNCTIONS TO ALLOW THEM TO WORK
        WallSlide();
        WallJump();

        // FLIPS THE CHARACTER (WHICH IS USED FOR MOVEMENT AND WALL JUMPING)
        if(!isWallJumping) 
        {        
            Flip();
        }


        if (Input.GetKeyDown(KeyCode.Space) && canDash) 
        {
            StartCoroutine(Dash());
        }

    }

    
    private void FixedUpdate() 
    {
        if (isDashing)
        {
            return;
        }
        
        // LEFT AND RIGHT MOVEMENT. BASIC STUFF
        if(!isWallJumping) 
        {       
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }


    // CHECKS IF THE PLAYER IS GROUNDED USING AN EMPTY OBJECT THAT IS CONNECTED TO THE BOTTOM OF THE PLAYER
    private bool IsGrounded() 
    {  
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // CHECKS IF THE PLAYER IS HUGGING A WALL USING AN EMPTY OBJECT THAT IS CONNECTED TO THE RIGHT OF THE PLAYER (THIS OBJECT FLIPS TO THE LEFT USING THE FLIP FUNCTION)
    private bool IsOnWall() 
    {  
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    // FUNCTION FOR THE WALL SLIDE
    private void WallSlide() 
    {
    
        // CHECKS IF PLAYER IS HUGGING WALL AND IS NOT GROUNDED, HALVING THE SPEED OF THEIR DESCENT AS LONG AS THEY ARE HUGGING THE WALL
        if (IsOnWall() && !IsGrounded() && horizontalInput != 0f) 
        {    
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else 
        {      
            isWallSliding = false;
        }

    }

    // FUNCTION FOR THE WALL JUMP
    private void WallJump() 
    {
    
        // CHECKS IF THE PLAYER IS WALL SLIDING. IF THEY ARE, THEY CAN JUMP OFF THE WALL, FLIPPING THEM AND APPLYING THE STANDARD JUMP FORCE
        if (isWallSliding) 
        {        
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else 
        {        
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && wallJumpCounter > 0f) 
        {           
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            if (transform.localScale.x != wallJumpDirection) 
            {
            
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            
            }

            Invoke(nameof(StopWallJumping), wallJumpDuration);     
        }
    
    }

    // USED TO PREVENT THE PLAYER FROM CONSTANTLY WALL JUMPING
    private void StopWallJumping() 
    {  
        isWallJumping = false;
    }

    // FLIPS THE PLAYER. PRETTY SELF-EXPLANATORY
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


    private IEnumerator Dash () 
    {
        canDash = false;

        float originalGravity = rb.gravityScale;

        isDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        
        canDash = true;
    }
}
