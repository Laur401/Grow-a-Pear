using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip failureSound; //TODO: Add failure
    private AudioSource audioSource;
    private HashSet<GameObject> players = new HashSet<GameObject>();
    void Awake() => SceneManager.sceneLoaded+=OnSceneLoaded;
    void Start() => audioSource = GetComponent<AudioSource>();

    private bool isRunning;
    void Update()
    {
        if (players.Count >= 2&&!isRunning)
            StartCoroutine(NextLevel());
    }

    public void AddPlayer(GameObject player) => players.Add(player);
    public void RemovePlayer(GameObject player) => players.Remove(player);
    private IEnumerator NextLevel()
    {
        isRunning = true;
        SimpleAudioManager.Manager.instance.StopSong(0.2f);
        audioSource.PlayOneShot(victorySound);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //replace with next level GUI
        isRunning = false;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => players.Clear();
}
