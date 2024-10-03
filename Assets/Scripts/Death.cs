using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Death : MonoBehaviour
{
    public List<Vector3> respawnPoints=new List<Vector3>();
    private GameObject[] players;
    
    private void Start()
    {
        players=GameObject.FindGameObjectsWithTag("Player");
        respawnPoints.Add(players[0].transform.position);
    }

    private void Update()
    {
        if (Input.GetButton("Respawn"))
        {
            foreach (GameObject player in players)
            {
                player.SetActive(true);
                player.transform.position = respawnPoints.Last();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.SetActive(false);
    }
}
