using UnityEngine;
using UnityEngine.Audio;

public class NewController2D : MonoBehaviour
{

    [Header("---------- Jump var ----------")]
    // Public
    public float minJumpHeight;
    public float maxJumpHeight;
    public int maxJumpNumber;
    public float timeToMaxHeight;

    public float apexThreshold;
    public float apexHangTime;

    // Private
    private float jumpHeight;
    private float baseHeight;

    private int jumpsUsed;
    private float timeOnJumping = 0;
    private float timeOnApex = 0;

    private float gravity;
    private float minJumpVelocity;
    private float maxJumpVelocity;
    private float jumpChargeRate;

    private bool isChargingJump = false;
    private float currentJumpVelocity;

    private Vector3 baseGravity;

    [Header("---------- Fall var ----------")]
    // Public
    public float fallMultiplier;

    // Private
    private Vector2 vecGravity;

    [Header("---------- Move var / grounded ----------")]
    // Public
    public float grounded_minWalkSpeed;
    public float grounded_maxWalkSpeed;
    public float grounded_runSpeed;
    public float grounded_AccelerationTimeToMaxSpeed;
    public float grounded_DecelerationTimeToStatic;
    public AnimationCurve grounded_AccelerationCurve;
    public AnimationCurve grounded_DecelerationCurve;

    // Private
    private float timeOnMovement = 0;
    private float timeOnSlowing = 0;

    private Vector2 VelocityOnStopping;

    [Header("---------- Move var / on air ----------")]
    // Public
    public float onAir_minWalkSpeed;
    public float onAir_maxWalkSpeed;
    public float onAir_runSpeed;
    public float onAir_AccelerationTimeToMaxSpeed;
    public float onAir_DecelerationTimeToStatic;
    public AnimationCurve onAir_AccelerationCurve;
    public AnimationCurve onAir_DecelerationCurve;

    // Private
    private bool wasMovingOnGround;

    [Header("---------- Can and Can't ----------")]
    public bool canMoveOnAir;
    public bool canDashOnAir;
    public bool canFallFaster;

    [Header("---------- States ----------")]
    public bool isOnGround;
    public bool isWalking;
    public bool isRunning;
    public bool isFalling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseGravity = Physics2D.gravity;
        vecGravity = new Vector2(0, - Physics2D.gravity.y);
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
    void FixedUpdate()
    {
        // variables
        Rigidbody2D rb = GetComponent<Rigidbody2D>();


        if (isOnGround)
        {
            ResetJumps();
            isFalling = false;
        }

        else if (rb.linearVelocity.y < 0)
        {
            isFalling = true;
            Fall(rb);
        }
    }

    #region Math
    public void Fall(Rigidbody2D rb)
    {
        if (canFallFaster)
        {
            rb.linearVelocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }
    #endregion

    #region Jump
    public void Jump(bool wasPressedThisFrame)
    {
        // variables
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float jumpVelocity;
        Vector2 velocity;

        // Calculus
        gravity = (-2 * maxJumpHeight) / (timeToMaxHeight * timeToMaxHeight);
        maxJumpVelocity = Mathf.Abs(gravity * timeToMaxHeight);
        minJumpVelocity = Mathf.Sqrt(2 * -gravity * minJumpHeight);

        jumpChargeRate = (maxJumpVelocity - minJumpVelocity) / timeToMaxHeight;

        Physics2D.gravity = new Vector2(0, gravity); // Optional: customize scene gravity

        // Initial Jump
        if (wasPressedThisFrame)
        {
            // Add 1 jump
            jumpsUsed ++;
            jumpHeight = minJumpHeight;
            baseHeight = gameObject.transform.position.y;

            if (jumpsUsed < maxJumpNumber)
            {
                isChargingJump = true;
                currentJumpVelocity = minJumpVelocity;

                // Immediately apply min jump velocity
                if(isWalking || isRunning)
                {
                    velocity = rb.linearVelocity;
                }
                else { velocity = Vector2.zero; }
                velocity.y = currentJumpVelocity;
                Debug.Log("velocity on Jump: " + velocity);
                rb.linearVelocity = velocity;
            }
        }

        // Jump Higher on holding
        if (jumpsUsed < maxJumpNumber && timeOnJumping <= timeToMaxHeight && gameObject.transform.position.y < baseHeight + maxJumpHeight)
        {
            
            float t = Mathf.Clamp01(timeOnJumping / timeToMaxHeight);

            jumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, timeOnJumping);
            // Debug.Log("jump height: " + jumpHeight + "      base height: " + baseHeight + "        posY: " + gameObject.transform.position.y);

            jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            if (isWalking || isRunning)
            {
                velocity = rb.linearVelocity;
            }
            else { velocity = Vector2.zero; }
            velocity.y = jumpVelocity;
            rb.linearVelocity = velocity;

            Debug.Log("velocity on Jump: " + velocity);

        }
        else if (jumpsUsed < maxJumpNumber && timeOnApex <= apexHangTime)
        {
            timeOnApex += Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            timeOnJumping = timeToMaxHeight + 1;

            Debug.Log("OnApex");
        }

        timeOnJumping += Time.deltaTime;

    }

    public void ResetJumps()
    {
        jumpsUsed = 0;
    }

    public void ReleaseJump()
    {
        isChargingJump = false;
        Physics2D.gravity = baseGravity;
        timeOnJumping = 0;
        timeOnApex = 0;
    }
    #endregion

    #region Move
    public void Move(Vector2 moveInput, bool runIsPressed)
    {
        // variables
        float maxSpeed;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 minVelocity;
        Vector2 maxVelocity;

        timeOnSlowing = 0;

        Debug.Log("move function entered");
        
        // On Ground
        if (isOnGround)
        {
            // Run
            if (runIsPressed) 
            {
                Debug.Log("running");
                maxSpeed = grounded_runSpeed; 
                isRunning = true;
                isWalking = false;
            }

            // Walk
            else 
            {
                Debug.Log("walking");
                maxSpeed = grounded_maxWalkSpeed; 
                isWalking = true;
                isRunning = false;
            }

            Debug.Log("should move");

            timeOnMovement = Mathf.Clamp(timeOnMovement += Time.deltaTime, 0, grounded_AccelerationTimeToMaxSpeed);

            // Move
            minVelocity = new Vector2(moveInput.x, 0f) * grounded_minWalkSpeed;
            maxVelocity = new Vector2(moveInput.x, 0f) * maxSpeed;

            float t = Mathf.Clamp01(timeOnMovement / grounded_AccelerationTimeToMaxSpeed);
            float shapedT = grounded_AccelerationCurve.Evaluate(t);

            Vector2 moveVelocity = Vector2.Lerp(minVelocity, maxVelocity, shapedT);
            rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);

            // Debug.Log("current velocity: " + rb.linearVelocity + "     time spent: " + timeOnMovement);
        }

        // On Air
        else
        {
            if (canMoveOnAir)
            {
                // Run
                if (runIsPressed)
                {
                    Debug.Log("running");
                    maxSpeed = onAir_runSpeed;
                    isRunning = true;
                    isWalking = false;
                }

                // Walk
                else
                {
                    Debug.Log("walking");
                    maxSpeed = onAir_maxWalkSpeed;
                    isWalking = true;
                    isRunning = false;
                }

                Debug.Log("should move");

                timeOnMovement = Mathf.Clamp(timeOnMovement += Time.deltaTime, 0, grounded_AccelerationTimeToMaxSpeed);

                // Move
                minVelocity = new Vector2(moveInput.x, 0f) * onAir_minWalkSpeed;
                maxVelocity = new Vector2(moveInput.x, 0f) * maxSpeed;

                float t = Mathf.Clamp01(timeOnMovement / onAir_AccelerationTimeToMaxSpeed);
                float shapedT = onAir_AccelerationCurve.Evaluate(t);

                Vector2 moveVelocity = Vector2.Lerp(minVelocity, maxVelocity, shapedT);
                rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);

                // Debug.Log("current velocity: " + rb.linearVelocity + "     time spent: " + timeOnMovement);
            }
        }
    }

    public void StopMoving()
    {
        // variables
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 minVelocity;

        Debug.Log("stop function entered");

        if (timeOnSlowing == 0)
        {
            VelocityOnStopping = rb.linearVelocity;
        }

        // Moving
        if (rb.linearVelocity.x != 0)
        {
            // Timers
            timeOnMovement = Mathf.Clamp(Mathf.Clamp01(timeOnSlowing / grounded_DecelerationTimeToStatic) * grounded_AccelerationTimeToMaxSpeed, 0, timeOnMovement);
            timeOnSlowing += Time.deltaTime;

            // On Ground
            if (isOnGround)
            {
                wasMovingOnGround = true;

                // Decelerate
                minVelocity = new Vector2(VelocityOnStopping.x, 0f);

                float t = Mathf.Clamp01(timeOnSlowing / grounded_DecelerationTimeToStatic);
                float shapedT = grounded_DecelerationCurve.Evaluate(t);

                // Debug.Log("shapedT : " + shapedT);

                Vector2 moveVelocity = Vector2.Lerp(minVelocity, Vector2.zero, shapedT);
                rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);

                // Debug.Log("current velocity: " + rb.linearVelocity + "     time spent: " + timeOnSlowing);
            }

            // On Air
            else
            {
                if (wasMovingOnGround)
                {
                    timeOnSlowing = onAir_DecelerationTimeToStatic * (timeOnSlowing / grounded_DecelerationTimeToStatic);
                    wasMovingOnGround = false;
                }
                // Decelerate
                minVelocity = new Vector2(VelocityOnStopping.x, 0f);

                float t = Mathf.Clamp01(timeOnSlowing / onAir_DecelerationTimeToStatic);
                float shapedT = onAir_DecelerationCurve.Evaluate(t);

                Vector2 moveVelocity = Vector2.Lerp(minVelocity, Vector2.zero, shapedT);
                rb.linearVelocity = new Vector2(moveVelocity.x, rb.linearVelocity.y);

                // Debug.Log("current velocity: " + rb.linearVelocity + "     time spent: " + timeOnSlowing);
            }
        }

        // Not Moving
        else
        {
            isWalking = false;
        }
    }
    #endregion
}
