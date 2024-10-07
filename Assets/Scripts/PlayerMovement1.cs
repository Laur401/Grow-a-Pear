using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Change input keys to not hardcoded ones DONE
//TODO: Merge movement scripts into one DONE
//TODO: Change growth/shrink into one float and treat it as N and 1/N respectively DONE
public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sizeChangeFactor;
    [SerializeField] private float maxSize = 1.0f;
    [SerializeField] private GameObject otherPlayer;
    [SerializeField] private float throwForce;

    private Rigidbody2D body;
    private CapsuleCollider2D capsule;
    private BoxCollider2D feet;

    //private Animator anim;
    private bool grounded;
    private bool canThrowPlayer1 = true;
    private bool canThrowPlayer2 = true;

    private Vector2 moveInput;
    private float growShrinkInput;

    private void Start()
    {
        // Grab references for Rigidbody and Animator from the object
        body = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        feet = GetComponent<BoxCollider2D>();
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        FlipSprite();
        ChangeSize();
        //HandleThrowing();
    }
    
    private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    private void OnGrowShrink(InputValue value) => growShrinkInput = value.Get<float>();

    private void OnJump(InputValue value)
    {
        if (!feet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
            body.velocity += new Vector2(0f, jumpForce);
    }
    
    private void HandleMovement()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, body.velocity.y);
        body.velocity = playerVelocity;

        // Set animator parameters
        //anim.SetBool("run", horizontalInput != 0);
        //anim.SetBool("grounded", grounded);

        //HandleThrowing();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(body.velocity.x), 1.0f);
    }

    private void ChangeSize()
    {
        float playerSizeX=Mathf.Clamp(transform.localScale.x*Mathf.Pow(sizeChangeFactor,growShrinkInput),1/maxSize,maxSize);
        float playerSizeY=Mathf.Clamp(transform.localScale.y*Mathf.Pow(sizeChangeFactor,growShrinkInput),1/maxSize,maxSize);
        float otherPlayerSizeX=Mathf.Clamp(otherPlayer.transform.position.x*sizeChangeFactor,1/maxSize,maxSize);
        float otherPlayerSizeY=Mathf.Clamp(otherPlayer.transform.position.y*sizeChangeFactor,1/maxSize,maxSize);
        Vector3 playerSize = new Vector3(playerSizeX,playerSizeY,1);
        Vector3 otherPlayerSize = new Vector3(otherPlayerSizeX,otherPlayerSizeY,1);
        transform.localScale = playerSize;
        otherPlayer.transform.localScale = otherPlayerSize;
    }

    private void Grow(GameObject player, GameObject otherPlayer)
    {
        Vector3 newScale = new Vector3(transform.localScale.x * sizeChangeFactor,
            transform.localScale.y * sizeChangeFactor, 1);
        Vector3 otherNewScale = new Vector3(otherPlayer.transform.localScale.x * (1 / sizeChangeFactor),
            otherPlayer.transform.localScale.y * (1 / sizeChangeFactor), 1);

        if (newScale.y <= maxSize && otherNewScale.y >= 1 / maxSize) // Set a max scale limit
        {
            player.transform.localScale = newScale; // Adjust scaling
            otherPlayer.transform.localScale = otherNewScale;
        }
    }

    private void Shrink(GameObject player, GameObject otherPlayer)
    {
        Vector3 newScale = new Vector3(transform.localScale.x * (1 / sizeChangeFactor),
            transform.localScale.y * (1 / sizeChangeFactor), 1);
        Vector3 otherNewScale = new Vector3(otherPlayer.transform.localScale.x * sizeChangeFactor,
            otherPlayer.transform.localScale.y * sizeChangeFactor, 1);
        if (newScale.y >= 1 / maxSize && otherNewScale.y <= maxSize) // Set a min scale limit
        {
            player.transform.localScale = newScale; // Return to normal size
            otherPlayer.transform.localScale = otherNewScale;
        }
    }

} /*    private void HandleThrowing() //I commented this out because inputs weren't working with this for some reason
    {
        float distance = Vector2.Distance(Player1.transform.position, Player2.transform.position);
        float maxThrowDistance = 5f;

        // Debug log to confirm distance
        //Debug.Log($"Distance between players: {distance}");

        // Check if within throw distance
        if (distance <= maxThrowDistance)
        {
            float Player1Scale = Player1.transform.localScale.y;
            float Player2Scale = Player2.transform.localScale.y;

            if (Input.GetKey(KeyCode.V) && Player1Scale > Player2Scale && grounded)
            {
                Debug.Log("Player 1 throwing Player 2");
                Throw(Player1, Player2);
                canThrowPlayer1 = false;
            }

            if (Input.GetKey(KeyCode.Slash) && Player2Scale > Player1Scale && grounded)
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

        float horizontalThrowForce = throwDirection.x * throwForce;
        float verticalThrowForce = throwForce * 0.5f;

        // Apply throw force
        Vector2 throwVelocity = new Vector2(horizontalThrowForce, verticalThrowForce); // Add some upward force
        thrownBody.velocity = throwVelocity;

        // Debug
        Debug.Log($"{thrower.name} threw {thrown.name} with force: {throwVelocity}");
    }
}*/