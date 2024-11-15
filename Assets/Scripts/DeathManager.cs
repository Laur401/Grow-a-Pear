using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;  
using UnityEngine.Rendering;

public class DeathManager : MonoBehaviour
{
    public List<Vector3> respawnPoints=new List<Vector3>();
    private GameObject[] players;
    
    [SerializeField] InputActionAsset inputActionAsset;
    enum Players { Player1, Player2 };
    [SerializeField] Players playerName;
    private InputActionMap player;
    private InputAction respawn;

    [SerializeField] private GameObject playerObject;

    private bool canRespawn = true;
    
    HealthUI healthUI;
    
    private void Start()
    {
        respawnPoints.Add(playerObject.transform.position);
        inputActionAsset.Enable();
        player = inputActionAsset.FindActionMap($"{playerName.ToString()}");
        respawn = player.FindAction("Respawn");
        
        healthUI = FindFirstObjectByType<HealthUI>();
    }

    private void Update()
    {
        OnRespawn(respawn);
    }

    void OnRespawn(InputAction value)
    {
        if (value.triggered&&canRespawn&&!playerObject.activeSelf)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    public bool alreadyRunning=false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadly")&&!alreadyRunning)
        {
            StartCoroutine(KillPlayer());
        }
    }
    
    public IEnumerator KillPlayer()
    {
        alreadyRunning = true;
        healthUI.RemoveLife();
        playerObject.SetActive(false);
        alreadyRunning = false;
        yield return null;
        //canRespawn=true; //Do we want them to respawn only after death?
    }

    private IEnumerator RespawnPlayer()
    {
        playerObject.SetActive(true);
        playerObject.transform.position = respawnPoints.Last();
        playerObject.GetComponent<PlayerMovement1>().canJump = true;
        canRespawn = false;
        yield return new WaitForSeconds(0.5f);
        canRespawn = true;
    }
}
