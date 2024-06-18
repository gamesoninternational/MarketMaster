using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // Rotation speed in degrees per second
    public Animator animator;

    private Vector2 moveInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the player.");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component not assigned.");
        }
    }

    void Update()
    {
        // Update the animator parameters
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // Move the player
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.velocity = moveDirection * moveSpeed;

        // Rotate the player to face the move direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Function to receive input from the joystick
    public void OnMoveInput(Vector2 input)
    {
        moveInput = input;
        Debug.Log("Move Input: " + moveInput);
    }

    // Function to update animation based on movement input
    void UpdateAnimation()
    {
        bool isWalking = moveInput.sqrMagnitude > 0;
        animator.SetBool("IsWalking", isWalking);
        //Debug.Log("IsWalking: " + isWalking);
    }
}
