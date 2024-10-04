using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    //TODO: Change input keys to not hardcoded ones
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float growFactor;
    [SerializeField] private float shrinkFactor;
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    [SerializeField] private float throwForce;

    private Rigidbody2D body;
    //private Animator anim;
    private bool grounded;
    private bool canThrowPlayer1 = true;
    private bool canThrowPlayer2 = true;

    private void Awake()
    {
        // Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Player 1 specific controls - Using custom axis "HorizontalP2"
        float horizontalInput = Input.GetAxis("HorizontalP2");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player when moving left or right, maintaining the current scale
        if (horizontalInput > 0.01f)
        {
            // Facing right, keep the current x scale
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            // Facing left, keep the current x scale
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }

        if (Input.GetKey(KeyCode.UpArrow) && grounded)
            Jump();
        if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1;
        if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1;
        if (Input.GetKeyDown(KeyCode.Comma))
            Grow(Player2, Player1);
        if (Input.GetKeyDown(KeyCode.Period))
            Shrink(Player2, Player1);

        // Set animator parameters
        //anim.SetBool("run", horizontalInput != 0);
        //anim.SetBool("grounded", grounded);

        HandleThrowing();
    }

    private void Jump()
    {
        float adjustedJumpForce = jumpForce;

        if(transform.localScale.y > 1.0f)
        {
            adjustedJumpForce = adjustedJumpForce * transform.localScale.y;
        }

        if(transform.localScale.y < 1.0f)
        {
            adjustedJumpForce = adjustedJumpForce * transform.localScale.y;
        }

        body.velocity = new Vector2(body.velocity.x, adjustedJumpForce);
        //anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            //anim.SetBool("grounded", true);
        }

        canThrowPlayer1 = true;
        canThrowPlayer2 = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            //anim.SetBool("grounded", false);  // Update animator when leaving the ground
        }
    }

    private void Grow(GameObject player, GameObject otherPlayer)
    {
        // Only change the y scale, maintain the x scale
        Vector3 newScale = new Vector3(player.transform.localScale.x * growFactor, player.transform.localScale.y * growFactor, 1);
        Vector3 otherNewScale = new Vector3(otherPlayer.transform.localScale.x * shrinkFactor, otherPlayer.transform.localScale.y * shrinkFactor, 1);
        if (newScale.y <= 2.4f && otherNewScale.y >= 0.1f) // Set a max scale limit
        {
            player.transform.localScale = newScale; // Adjust scaling
            otherPlayer.transform.localScale = otherNewScale;
        }
    }

    private void Shrink(GameObject player, GameObject otherPlayer)
    {
        // Only change the y scale, maintain the x scale
        Vector3 newScale = new Vector3(player.transform.localScale.x * shrinkFactor, player.transform.localScale.y * shrinkFactor, 1);
        Vector3 otherNewScale = new Vector3(otherPlayer.transform.localScale.x * growFactor, otherPlayer.transform.localScale.y * growFactor, 1);
        if (newScale.y >= 0.1f && otherNewScale.y <= 2.4f) // Set a min scale limit
        {
            player.transform.localScale = newScale; // Return to normal size
            otherPlayer.transform.localScale = otherNewScale;
        }
    }

        private void HandleThrowing()
    {
        float distance = Vector2.Distance(Player1.transform.position, Player2.transform.position);
        float maxThrowDistance = 1.5f;

        if (distance <= maxThrowDistance)
        {
            float Player1Scale = Player1.transform.localScale.y;
            float Player2Scale = Player2.transform.localScale.y;

        if (Input.GetKey(KeyCode.V) && Player1Scale > Player2Scale)
        {
            Debug.Log("Player 1 throwing Player 2");
            Throw(Player1, Player2);
            canThrowPlayer1 = false;
        }

        if (Input.GetKey(KeyCode.Slash) && Player2Scale > Player1Scale)
        {
            Debug.Log("Player 2 throwing Player 1");
            Throw(Player2, Player1);
            canThrowPlayer2 = false;
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

        // Apply throw force
        Vector2 throwVelocity = new Vector2(throwDirection.x * throwForce, throwForce * 0.75f);  // Add some upward force
        thrownBody.velocity = throwVelocity;

        // Debug
        Debug.Log($"{thrower.name} threw {thrown.name} with force: {throwVelocity}");
    }
}