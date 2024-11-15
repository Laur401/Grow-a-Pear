using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int attackDamage = 20;
    public GameObject fireballPrefab;
    public Vector2 fireballDirection = Vector2.right;
    public float fireballSpeed = 5f;

    public Vector3 attackOffset;
    public float attackRange = 5f;
    public LayerMask attackMask;

    private bool isMeleeAttack = true; // Variable to alternate between attacks

    // Call this function from the Animator's attack trigger
public void Attack()
{
    if (isMeleeAttack)
    {
        Debug.Log("Performing Melee Attack");
        MeleeAttack();
    }
    else
    {
        Debug.Log("Shooting Fireball");
        ShootFireball();

        // Trigger Attack2 animation for the boss
        Animator bossAnimator = GetComponent<Animator>();
        if (bossAnimator != null)
        {
            Debug.Log("Triggering Attack2 animation");
            bossAnimator.SetTrigger("attack2");
        }
        else
        {
            Debug.LogWarning("Animator not found!");
        }
    }

    // Toggle the attack type for the next attack
    isMeleeAttack = !isMeleeAttack;
}

    private void MeleeAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    private void ShootFireball()
    {
        // Instantiate the fireball at the boss's position with an offset for visibility
        Vector3 fireballSpawnPosition = transform.position + new Vector3(attackOffset.x, attackOffset.y + 1.0f, 0);
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPosition, Quaternion.identity);

        // Check if the fireball was instantiated and log its position
        if (fireball != null)
        {
            Debug.Log("Fireball spawned at position: " + fireballSpawnPosition);
        }

        // Set the fireball's velocity in the specified direction
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        if (fireballRb != null)
        {
            fireballRb.velocity = fireballDirection.normalized * fireballSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}