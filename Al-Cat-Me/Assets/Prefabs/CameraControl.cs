using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Ensure Rigidbody2D is set up correctly
        rb.gravityScale = 0f; // No gravity for top-down movement
        rb.freezeRotation = true; // Prevent unwanted rotation
    }

    void Update()
    {
        // Get input from Unity's Input system (WASD / Arrow keys)
        float moveX = Input.GetAxisRaw("Horizontal"); // -1, 0, or 1
        float moveY = Input.GetAxisRaw("Vertical");   // -1, 0, or 1

        // Normalize diagonal movement to prevent faster speed
        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for physics consistency
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
