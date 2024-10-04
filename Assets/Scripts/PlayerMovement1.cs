using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float growFactor;
    [SerializeField] private float shrinkFactor;
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    [SerializeField] private float throwForce;

    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        // Grab references for Rigidbody and Animator from the object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleThrowing();
    }

    private void HandleMovement()
    {
        // Example Player 1 specific controls
        // Uncomment and modify as needed for Player 1 controls
        float horizontalInput = Input.GetAxis("HorizontalP1");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player when moving left or right, maintaining the current scale
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }

        // Jump logic
        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        // Scaling logic
        if (Input.GetKeyDown(KeyCode.Z))
            Grow();
        if (Input.GetKeyDown(KeyCode.X))
            Shrink();

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        float adjustedJumpForce = jumpForce * transform.localScale.y; // Adjust jump force based on scale
        body.velocity = new Vector2(body.velocity.x, adjustedJumpForce);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            anim.SetBool("grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            anim.SetBool("grounded", false);  // Update animator when leaving the ground
        }
    }

    private void Grow()
    {
        Vector3 newScale = new Vector3(transform.localScale.x * growFactor, transform.localScale.y * growFactor, 1);
        if (newScale.y <= 3.0f) // Set a max scale limit
        {
            transform.localScale = newScale; // Adjust scaling
        }
    }

    private void Shrink()
    {
        Vector3 newScale = new Vector3(transform.localScale.x * shrinkFactor, transform.localScale.y * shrinkFactor, 1);
        if (newScale.y >= 0.5f) // Set a min scale limit
        {
            transform.localScale = newScale; // Return to normal size
        }
    }

    private void HandleThrowing()
    {
        float distance = Vector2.Distance(Player1.transform.position, Player2.transform.position);
        float maxThrowDistance = 5f;

        // Debug log to confirm distance
        Debug.Log($"Distance between players: {distance}");

        // Check if within throw distance
        if (distance <= maxThrowDistance)
        {
            float Player1Scale = Player1.transform.localScale.y;
            float Player2Scale = Player2.transform.localScale.y;

            if (Input.GetKey(KeyCode.V) && Player1Scale > Player2Scale && grounded)
            {
                Debug.Log("Player 1 throwing Player 2");
                Throw(Player1, Player2);
            }

            if (Input.GetKey(KeyCode.Slash) && Player2Scale > Player1Scale && grounded)
            {
                Debug.Log("Player 2 throwing Player 1");
                Throw(Player2, Player1);
            }
        }
    }

    private void Throw(GameObject thrower, GameObject thrown)
    {
        Rigidbody2D thrownBody = thrown.GetComponent<Rigidbody2D>();

        if (thrownBody == null)
        {
            Debug.LogError("Thrown object does not have a Rigidbody2D component.");
            return;
        }

        // Determine direction based on thrower's facing direction
        Vector2 throwDirection = thrower.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        float horizontalThrowForce = throwDirection.x * throwForce;
        float verticalThrowForce = throwForce * 0.5f;

        // Apply throw force
        Vector2 throwVelocity = new Vector2(horizontalThrowForce, verticalThrowForce); // Add some upward force
        thrownBody.velocity = throwVelocity;

        // Debug
        Debug.Log($"{thrower.name} threw {thrown.name} with force: {throwVelocity}");
    }
}
