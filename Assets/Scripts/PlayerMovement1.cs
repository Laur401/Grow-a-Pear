using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

//TODO: Change input keys to not hardcoded ones DONE
//TODO: Merge movement scripts into one DONE
//TODO: Change growth/shrink into one float and treat it as N and 1/N respectively DONE
public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpBoost=1f;
    [SerializeField] private float sizeChangeFactor;
    [SerializeField] private float maxSize = 1.0f;
    [SerializeField] private GameObject otherPlayer;
    [SerializeField] private float throwForce;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private Rigidbody2D body;
    private CapsuleCollider2D capsule;
    private BoxCollider2D feet;

    //private Animator anim;
    private bool grounded;
    private bool canThrowPlayer1 = true;
    private bool canThrowPlayer2 = true;

    private Vector2 moveInput;
    private float growShrinkInput;
    private int direction;
    private float targetVelocity;
    private float coyoteTimer;
    private float bufferTimer;
    private bool canJump=true;

    private Pickup heldItem;

    enum Players { Player1, Player2 };
    [SerializeField] Players playerName;
    private InputActionMap player;
    private InputAction move;
    private InputAction jump;
    private InputAction grab;
    private InputAction growShrink;
    private InputAction interact;

    [NonSerialized] public Vector2 extraSpeed = new Vector2(0, 0);

    private void Start()
    {
        // Grab references for Rigidbody and Animator from the object
        body = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        feet = GetComponent<BoxCollider2D>();
        //anim = GetComponent<Animator>();
        inputActionAsset.Enable();
        player = inputActionAsset.FindActionMap($"{playerName.ToString()}");
        move = player.FindAction("Move");
        jump = player.FindAction("Jump");
        grab = player.FindAction("Grab");
        growShrink = player.FindAction("GrowShrink");
        interact = player.FindAction("Interact");
        jump.performed += OnJump;
        //grab.performed += OnGrab;
        //interact.performed += OnInteract;
        StartCoroutine(LogDisplay());
    }

    IEnumerator LogDisplay()
    {
        bool bef;
        while (true)
        {
            bef = jump.triggered;
            Debug.Log($"jump.triggered: {jump.triggered}");
            yield return new WaitUntil(() => jump.triggered != bef);
        }
    }

    private void Update()
    {
        InputChecker();
        ChangeSize();
        FlipSprite();
        DebugFunction();
        if (heldItem)
            ItemHolding();
        //HandleThrowing();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        BufferJump(jump);
    }

    private void InputChecker()
    {
        OnMove(move);
        OnGrowShrink(growShrink);
    }

    private void OnMove(InputAction value) => moveInput = value.ReadValue<Vector2>();
    private void OnGrowShrink(InputAction value) => growShrinkInput = value.ReadValue<float>();

    private bool coroutineIsCalled = false;
    private void BufferJump(InputAction value)
    {
        if (!feet.IsTouchingLayers(LayerMask.GetMask("Ground", "Player", "Object")))
        {
            coyoteTimer -= Time.deltaTime;
            bufferTimer -= Time.deltaTime;
            if (coyoteTimer <= 0 && !coroutineIsCalled)
                StartCoroutine(CanJump());
        }
        else
        {
            coyoteTimer = coyoteTime;
            if (canJump && bufferTimer > 0)
            {
                Jump();
                Debug.Log("Buffer jump");
            }
        }
    }
    
    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!feet.IsTouchingLayers(LayerMask.GetMask("Ground", "Player", "Object")))
        {
            if (canJump && coyoteTimer > 0)
            {
                Jump();
                Debug.Log("Coyote jump");
                return;
            }
            bufferTimer = jumpBufferTime;
        }
        else if (canJump)
        {
            Jump();
            Debug.Log("Normal jump");
        }
    }

    private void Jump()
    {
        float jumpHeight = (2 / transform.localScale.y) + jumpBoost;
        body.AddForce(Mathf.Sqrt(jumpHeight * -2 * Physics2D.gravity.y) * transform.up.normalized,
            ForceMode2D.Impulse);
        //Debug.Log($"AddForce: {Mathf.Sqrt(jumpHeight * -2 * Physics2D.gravity.y)}, jumpHeight: {jumpHeight}, localScale: {transform.localScale.y}, jumpBoost: {jumpBoost}");
        if (!coroutineIsCalled)
            StartCoroutine(CanJump());
        coyoteTimer = 0;
        bufferTimer = 0;
    }

    IEnumerator CanJump()
    {
        coroutineIsCalled = true;
        canJump = false;
        Debug.Log("NO jump");
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(()=>feet.IsTouchingLayers(LayerMask.GetMask("Ground", "Player", "Object")));
        canJump = true;
        Debug.Log("YES jump");
        coroutineIsCalled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Pickup pickup = other.GetComponent<Pickup>();
        Lever lever = other.GetComponent<Lever>();
        if (pickup&&!heldItem)
        {
            if (pickup.PickUpHandler(grab, gameObject) == 1)
            {
                heldItem = pickup;
                Debug.Log("Picked up!");
            }
        }
        if (lever)
            lever.LeverFlipHandler(interact);
    }

    /*private List<Collider2D> triggerObject=new List<Collider2D>();
    void OnTriggerEnter2D (Collider2D other)
    {
        triggerObject.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        triggerObject.Remove(other);
    }

    private void OnGrab(InputAction.CallbackContext obj)
    {
        Pickup pickup=triggerObject.FindLast(x=>x.GetComponent<Pickup>()==true).GetComponent<Pickup>();
        if (pickup && pickup.PickUpHandler(grab, gameObject) == 1)
        {
            heldItem = pickup;
            Debug.Log("Picked up!");
        }
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        foreach (Collider2D coll in triggerObject)
        {
            Lever lever = coll.GetComponent<Lever>();
            if (lever)
                lever.LeverFlipHandler(interact);
        }
    }
    */

    private void ItemHolding()
    {
        if (heldItem.PickUpHandler(grab, gameObject) == 2)
        {
            heldItem = null;
            Debug.Log("Unpicked up!");
        }
    }

    private void HandleMovement()
    {
        if (move.IsPressed())
            body.velocity = new Vector2(moveInput.x * speed, body.velocity.y);
        else body.velocity = new Vector2(0f, body.velocity.y);
        //body.AddForce(new Vector2(moveInput.x*speed*Time.deltaTime,0),ForceMode2D.Impulse);
        if (extraSpeed != Vector2.zero)
        {
            //body.velocity += extraSpeed;
            body.AddForce(extraSpeed,ForceMode2D.Force);
            extraSpeed = Vector2.zero;
        }
        else if (feet.IsTouchingLayers(LayerMask.GetMask("Ground","Player","Object")))
            body.velocity=Vector2.MoveTowards(body.velocity,Vector2.zero,1.2f);
        
        // Set animator parameters
        //anim.SetBool("run", horizontalInput != 0);
        //anim.SetBool("grounded", grounded);

        //HandleThrowing();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
            transform.localScale=new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(moveInput.x),transform.localScale.y);
    }

    private void ChangeSize()
    {
        bool playerIsChangingSize = Mathf.Abs(growShrinkInput) > Mathf.Epsilon;
        Bounds bounds = gameObject.GetComponent<CapsuleCollider2D>().bounds;
        Bounds boundsOther = gameObject.GetComponent<CapsuleCollider2D>().bounds;
        //List<RaycastHit2D> colliders = new List<RaycastHit2D>();
        //List<RaycastHit2D> collidersOther = new List<RaycastHit2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = LayerMask.GetMask("Ground");
        //Physics2D.OverlapArea((Vector2)bounds.min+new Vector2(Mathf.Epsilon, Mathf.Epsilon), (Vector2)bounds.max * sizeChangeFactor, contactFilter, colliders);
        bool colliders = Physics2D.Raycast(new Vector2(bounds.center.x,bounds.max.y), Vector2.up, transform.localScale.y*(sizeChangeFactor-1), LayerMask.GetMask("Ground"));
        bool collidersOther = Physics2D.Raycast(new Vector2(boundsOther.center.x,boundsOther.max.y), Vector2.up, otherPlayer.transform.localScale.y*(sizeChangeFactor-1), LayerMask.GetMask("Ground"));
        Debug.DrawRay(new Vector2(bounds.center.x,bounds.max.y), transform.localScale.y*(sizeChangeFactor-1)*Vector2.up, Color.red);
        /*if (playerName==Players.Player1)
            foreach (RaycastHit2D hit in colliders)
                Debug.Log(hit.collider.name);
            //Debug.Log($"{colliders[0]}, {colliders[1]}");*/
        if (playerIsChangingSize&&!colliders&&!collidersOther)
        {
            float playerSizeX=Mathf.Clamp(Mathf.Abs(transform.localScale.x)*Mathf.Pow(sizeChangeFactor,growShrinkInput),1/maxSize,maxSize)*Mathf.Sign(transform.localScale.x);
            float playerSizeY=Mathf.Clamp(Mathf.Abs(transform.localScale.y)*Mathf.Pow(sizeChangeFactor,growShrinkInput),1/maxSize,maxSize)*Mathf.Sign(transform.localScale.y);
            float otherPlayerSizeX=Mathf.Clamp(Mathf.Abs(otherPlayer.transform.localScale.x)*Mathf.Pow(sizeChangeFactor,-growShrinkInput),1/maxSize,maxSize)*Mathf.Sign(otherPlayer.transform.localScale.x); //TODO: Fix direction and inverse scale for the other player (possibly just call its ChangeSize function?)
            float otherPlayerSizeY=Mathf.Clamp(otherPlayer.transform.localScale.y*Mathf.Pow(sizeChangeFactor,-growShrinkInput),1/maxSize,maxSize)*Mathf.Sign(otherPlayer.transform.localScale.y);
            Vector3 playerSize = new Vector3(playerSizeX,playerSizeY,1);
            Vector3 otherPlayerSize = new Vector3(otherPlayerSizeX,otherPlayerSizeY,1);
            transform.localScale = playerSize;
            otherPlayer.transform.localScale = otherPlayerSize;
            
        }
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
    
    private void DebugFunction()
    {
        if (Input.GetKey(KeyCode.BackQuote))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (Input.GetKey(KeyCode.Home))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        if (Input.GetKey(KeyCode.PageUp))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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