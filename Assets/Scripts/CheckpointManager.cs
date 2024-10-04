using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Death respawnPointsScript;

    void Start()
    {
        respawnPointsScript = FindObjectOfType<Death>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            respawnPointsScript.respawnPoints.Add(transform.position);
            
    }
}
