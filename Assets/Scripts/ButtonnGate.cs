using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonnGate : MonoBehaviour
{
    [SerializeField]GameObject gate;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag=="Player"||other.gameObject.tag=="Box")
        {
            gate.SetActive(false);
        }
    }
}   
