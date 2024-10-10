using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private List<GameObject> teleporters;
    [SerializeField] private bool canTeleportPlayers;
    [SerializeField] private bool canTeleportObjects;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTeleportPlayers&&other.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            int index = teleporters.IndexOf(gameObject);
            StartCoroutine(moveToNextTeleporter(other, index));
        }
        if (canTeleportObjects&&other.gameObject.layer==LayerMask.NameToLayer("Object"))
        {
            int index = teleporters.IndexOf(gameObject);
            StartCoroutine(moveToNextTeleporter(other, index));
        }
    }
    
    IEnumerator moveToNextTeleporter(Collider other, int index)
    {
        yield return new WaitForSeconds(3.0f);
        int nextTeleporter=(index+1)%teleporters.Count;
        other.gameObject.transform.position = teleporters[nextTeleporter].transform.position;
        //TODO: Make sure it doesn't teleport again after teleporting
    }
}
