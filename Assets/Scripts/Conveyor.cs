using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour, IActivator
{
    [SerializeField] private float speed = 20f;

    private bool active = false;
    public bool Active
    {
        get => active;
        set => active = value;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.isTrigger) return;
        
        MoveObject(other);
        /*if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
            MoveObject(other,true);
        if (other.gameObject.layer==LayerMask.NameToLayer("Object"))
            MoveObject(other,false);*/
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement1>()!=null)
            other.gameObject.GetComponent<PlayerMovement1>().extraSpeed = Vector2.zero;
    }

    void MoveObject(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement1>() != null)
            other.gameObject.GetComponent<PlayerMovement1>().extraSpeed = -transform.up.normalized * speed;
        else
        {
            Rigidbody2D rb;
            if (other.gameObject.GetComponent<Rigidbody2D>() != null)
                rb = other.gameObject.GetComponent<Rigidbody2D>();
            else rb = other.gameObject.GetComponentInParent<Rigidbody2D>();
            if (rb == null) return;
            rb.AddForce(-transform.up.normalized * speed, ForceMode2D.Force);
        }
    }
}
