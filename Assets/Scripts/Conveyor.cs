using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    private void OnCollisionStay2D(Collision2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(-transform.up.normalized * speed, ForceMode2D.Force);
        if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
            MoveObject(other, false);
        if (other.gameObject.layer==LayerMask.NameToLayer("Object"))
            MoveObject(other, true);
    }

    void MoveObject(Collision2D other, bool nested)
    {
        Rigidbody2D rb;
        if (nested)
            rb = other.gameObject.GetComponentInParent<Rigidbody2D>();
        else rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(-transform.up.normalized * speed, ForceMode2D.Force);
    }
    //TODO: Maybe add an extra kick with OnCollisionExit2D?
    //TODO: Add a turn on/off function, connect with Activator
}
