using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [NonSerialized] public int playerCountAtFinish = 0;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip failureSound; //TODO: Add failure
    private AudioSource audioSource;
    void Awake() => SceneManager.sceneLoaded+=OnSceneLoaded;
    void Start() => audioSource = GetComponent<AudioSource>();
    void Update()
    {
        if (playerCountAtFinish >= 2)
            StartCoroutine(NextLevel());
    }
    private IEnumerator NextLevel()
    {
        SimpleAudioManager.Manager.instance.StopSong(0.5f);
        audioSource.PlayOneShot(victorySound);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //replace with next level GUI
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => playerCountAtFinish = 0;
}
