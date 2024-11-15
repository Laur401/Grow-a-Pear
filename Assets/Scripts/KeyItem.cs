using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Locked"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
