using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlagCounter : MonoBehaviour
{
    LevelFinish levelFinishScript;
    void Start()
    {
        levelFinishScript = GameObject.Find("Scene Manager").GetComponent<LevelFinish>();
    }
    
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
            levelFinishScript.AddPlayer(other.gameObject);
    }
    
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
            levelFinishScript.RemovePlayer(other.gameObject);
    }
}
