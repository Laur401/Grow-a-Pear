using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;


    public void Initialize(Vector2 targetPosition)
    {
        direction = (targetPosition - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            collision.GetComponent<PlayerHealth>().TakeDamage(20);
            Destroy(gameObject);
        }
    }
}
