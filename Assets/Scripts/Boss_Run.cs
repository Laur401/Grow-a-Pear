using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 2f;

    private Transform player1;
    private Transform player2;
    private Rigidbody2D rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get references to both players
        player1 = GameObject.Find("Bud").transform;
        player2 = GameObject.Find("Sprout").transform;

        rb = animator.GetComponent<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check which player is closer
        float distanceToPlayer1 = Vector2.Distance(rb.position, player1.position);
        float distanceToPlayer2 = Vector2.Distance(rb.position, player2.position);
        Transform closestPlayer = (distanceToPlayer1 < distanceToPlayer2) ? player1 : player2;

        // Move towards the closest player
        Vector2 target = new Vector2(closestPlayer.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Flip the boss to face the closest player
        if (closestPlayer.position.x < rb.position.x)
            animator.transform.localScale = new Vector3(1, 1, 1); // Face left
        else
            animator.transform.localScale = new Vector3(-1, 1, 1); // Face right

        // Trigger the attack if the closest player is within attack range
        if (Vector2.Distance(closestPlayer.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack");
    }
}
