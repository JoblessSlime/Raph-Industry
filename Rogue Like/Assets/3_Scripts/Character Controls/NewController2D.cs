using UnityEngine;
using UnityEngine.Audio;

public class NewController2D : MonoBehaviour
{

    [Header("---------- Jump var ----------")]
    // Public
    public float minJumpHeight;
    public float maxJumpHeight;
    public int maxJumpNumber;

    public float apexThreshold;
    public float apexHangTime;

    // Private
    private int jumpsUsed;

    [Header("---------- Move var ----------")]
    // Public
    public float minWalkSpeed;
    public float maxWalkSpeed;
    public float runSpeed;
    public float timeToMaxSpeed;

    [Header("---------- States ----------")]
    public bool isOnGround;
    public bool isMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnGround)
        {
            ResetJumps();
        }
    }

    #region Math
    #endregion

    public void Jump()
    {
        if(jumpsUsed < maxJumpNumber)
        {
            // Add 1 jump
            jumpsUsed += 1;

            // Jump
        }
    }

    public void Move(Vector2 moveInput, bool runIsPressed)
    {
        // variables
        float maxSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();

        // Run
        if(runIsPressed) { maxSpeed = runSpeed; }
        // Walk
        else { maxSpeed = maxWalkSpeed; }

        //Move
        Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * maxWalkSpeed;

        Vector2 moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);


        /*
        if (isGrounded && moveVelocity != Vector2.zero && !actionDone)
        {
            audioSource.Play();
            Debug.Log("playingSound");
            actionDone = true;
        }
        else if (actionDone)
        {
            audioSource.Pause();
            Debug.Log("stopingSound");
            actionDone = false;
        }

        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            // if(RUN), then same code but with run values
            targetVelocity = new Vector2(moveInput.x, 0f) * maxWalkSpeed;

            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            playerRB.linearVelocity = new Vector2(moveVelocity.x, playerRB.linearVelocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            playerRB.linearVelocity = new Vector2(moveVelocity.x, playerRB.linearVelocity.y);
        }
        */
    }

    public void ResetJumps()
    {
        jumpsUsed = 0;
    }
}
