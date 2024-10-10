using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crumbler : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Player")) != 0)
            Invoke(nameof(CollisionDisabler), 3f);
    }

    void CollisionDisabler()
    {
        //insert animation stuff here
        GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(CollisionEnabler),3f);
    }
    void CollisionEnabler()
    {
        GetComponent<Collider2D>().enabled = true;
    }
}
