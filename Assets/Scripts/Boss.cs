using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 2.5f;

    private Transform player1, player2;
    private Transform closestPlayer;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        // Find the players by their names in the hierarchy
        player1 = GameObject.Find("Bud").transform;
        player2 = GameObject.Find("Sprout").transform;
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player1 != null && player2 != null)
        {
            // Dynamically determine the closest player in every frame
            float distanceToPlayer1 = Vector2.Distance(rb.position, player1.position);
            float distanceToPlayer2 = Vector2.Distance(rb.position, player2.position);
            closestPlayer = (distanceToPlayer1 < distanceToPlayer2) ? player1 : player2;

            // Move towards the closest player
            Vector2 target = new Vector2(closestPlayer.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
            rb.MovePosition(newPos);

            // Flip the boss to face the closest player
            if (closestPlayer.position.x < rb.position.x)
                transform.localScale = new Vector3(1, 1, 1); // Face left
            else
                transform.localScale = new Vector3(-1, 1, 1); // Face right

            // Update the running animation based on movement
            animator.SetFloat("speed", Mathf.Abs(speed));
        }
    }
}
