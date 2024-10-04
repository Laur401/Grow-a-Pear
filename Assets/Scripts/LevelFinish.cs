using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private int playerCount = 0;
    // Start is called before the first frame update
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerCount++;
        if (playerCount >= 2)
            Invoke("NextLevel", 3.0f);
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerCount--;
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
