using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private DeathManager deathManager;
    private void Start()
    {
        deathManager = GetComponentInParent<DeathManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadly")&&!deathManager.alreadyRunning)
        {
            StartCoroutine(deathManager.KillPlayer());
        }
    }
}
