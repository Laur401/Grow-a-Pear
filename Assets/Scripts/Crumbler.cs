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
            StartCoroutine(CollisionChanger());
    }

    IEnumerator CollisionChanger()
    {
        yield return new WaitForSeconds(3f);
        //insert animation stuff here
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(3f);
        //insert animation stuff here
        GetComponent<Collider2D>().enabled = true;
    }
}
