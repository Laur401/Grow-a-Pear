using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Death : MonoBehaviour
{
    public List<Vector3> respawnPoints=new List<Vector3>();
    private GameObject[] players;
    
    [SerializeField] InputActionAsset inputActionAsset;
    enum Players { Player1, Player2 };
    [SerializeField] Players playerName;
    private InputActionMap player;
    private InputAction respawn;

    private bool canRespawn = true;
    
    private int livesLeft; //replace with connection to UI later
    
    private void Start()
    {
        respawnPoints.Add(transform.position);
        inputActionAsset.Enable();
        player = inputActionAsset.FindActionMap($"{playerName.ToString()}");
        respawn = player.FindAction("Respawn");
    }

    private void Update()
    {
        OnRespawn(respawn);
    }

    void OnRespawn(InputAction value)
    {
        if (value.triggered&&canRespawn)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadly"))
        {
            KillPlayer();
            livesLeft--; //TODO: Replace with connection to UI later
        }
    }

    private void KillPlayer()
    {
        gameObject.SetActive(false);
        //canRespawn=true; //Do we want them to respawn only after death?
    }

    private IEnumerator RespawnPlayer()
    {
        gameObject.SetActive(true);
        transform.position = respawnPoints.Last();
        canRespawn = false;
        yield return new WaitForSeconds(0.5f);
        canRespawn = true;
    }
}
