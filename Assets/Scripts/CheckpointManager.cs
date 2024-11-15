using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private DeathManager respawnPointsScript;

    void Start()
    {
        respawnPointsScript = GetComponentInParent<DeathManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
            respawnPointsScript.respawnPoints.Add(other.transform.position);
            
    }
}
