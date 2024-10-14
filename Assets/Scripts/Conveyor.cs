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
        //if (active)
        MoveObject(other);
        /*if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
            MoveObject(other);
        if (other.gameObject.layer==LayerMask.NameToLayer("Object"))
            MoveObject(other);*/
    }

    /*private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.isTrigger) return;
        if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
            ExtraKick(other);
        if (other.gameObject.layer==LayerMask.NameToLayer("Object"))
            ExtraKick(other);
    }*/

    void MoveObject(Collision2D other)
    {
        Rigidbody2D rb;
        Debug.Log("Hello");
        if (other.gameObject.GetComponent<Rigidbody2D>() != null)
            rb = other.gameObject.GetComponent<Rigidbody2D>();
        else rb = other.gameObject.GetComponentInParent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(-transform.up.normalized * speed, ForceMode2D.Impulse);
    }

    /*void ExtraKick(Collision2D other)
    {
        Rigidbody2D rb;
        if (other.gameObject.GetComponentInParent<Rigidbody2D>()!=null)
            rb = other.gameObject.GetComponentInParent<Rigidbody2D>();
        else rb = other.gameObject.GetComponent<Rigidbody2D>();
        //if (rb != null)
            
    }*/
    //TODO: Maybe add an extra kick with OnCollisionExit2D?
}
