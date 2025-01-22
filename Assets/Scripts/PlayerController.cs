using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;
    public float speed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    private float lastJumpPressTime;
    private float jumpBufferTime = 0.2f; // Allow jump input a small window before landing

    void Start()
    {
        // Get references
        rb = GetComponent<Rigidbody2D>();

        // Enable actions
        moveAction.Enable();
        jumpAction.Enable();

        // Bind the jump action callback
        jumpAction.performed += OnJumpInput;

        // Initialize lastJumpPressTime to a value that ensures it won't trigger a jump at start
        lastJumpPressTime = -100f;
    }

    void Update()
    {
        // Handle movement input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y); // Ensure rb.velocity is used properly

        // Handle jumping logic
        HandleJumping();
    }

    private void HandleJumping()
    {
        // If grounded and the jump button was pressed recently (using jump buffer)
        if (isGrounded && Time.time - lastJumpPressTime <= jumpBufferTime)
        {
            // Apply force for jumping
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Use rb.velocity instead of AddForce for 2D platformers
            lastJumpPressTime = -1; // Reset jump buffer after jumping
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Detect when the jump button was pressed
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        lastJumpPressTime = Time.time; // Record the time when jump button is pressed
    }
}
