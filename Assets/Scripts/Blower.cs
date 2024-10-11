using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    [SerializeField] private float blowForce = 20f;
    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(transform.up.normalized * blowForce, ForceMode2D.Force);
    }
}
