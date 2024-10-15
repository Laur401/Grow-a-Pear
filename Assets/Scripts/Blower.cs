using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour, IActivator
{
    [SerializeField] private float blowForce = 20f;
    [SerializeField] private bool isActive = true;
    private bool active;
    public bool Active
    {
        get => active;
        set => active = value;

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null&&active!=isActive)
            rb.AddForce(transform.up.normalized * blowForce, ForceMode2D.Force);
    }
}
