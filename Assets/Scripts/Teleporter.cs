using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private List<GameObject> teleporters;
    [SerializeField] private bool canTeleportPlayers;
    [SerializeField] private bool canTeleportObjects;
    private int nextTeleporterIndex;
    private Collider2D nextTeleporter;
    private Teleporter nextTeleporterScript;
    //public bool teleportedInto = false;
    public bool teleportedInto { get; set; }
    void Start()
    {
        int index = teleporters.IndexOf(gameObject);
        nextTeleporterIndex = (index + 1) % teleporters.Count;
        nextTeleporter = teleporters[nextTeleporterIndex].GetComponent<BoxCollider2D>();
        nextTeleporterScript = teleporters[nextTeleporterIndex].GetComponent<Teleporter>();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (teleportedInto||other.isTrigger) return;
        if (canTeleportPlayers&&other.gameObject.layer==LayerMask.NameToLayer("Player"))
            StartCoroutine(moveToNextTeleporter(other, false));
        if (canTeleportObjects&&other.gameObject.layer==LayerMask.NameToLayer("Object"))
            StartCoroutine(moveToNextTeleporter(other, true));
    }

    private void OnTriggerExit2D(Collider2D other) => teleportedInto = false;

    IEnumerator moveToNextTeleporter(Collider2D other, bool nested)
    {
        yield return new WaitForSeconds(3.0f);
        nextTeleporterScript.teleportedInto = true;
        if (nested)
            other.transform.parent.position = nextTeleporter.transform.position;
        else other.transform.position = nextTeleporter.transform.position;
    }
}
