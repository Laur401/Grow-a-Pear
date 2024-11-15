using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private Image transitionImage;
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
        Tween transition = transitionImage.DOFade(1f, 2f).SetDelay(1f).SetEase(Ease.OutSine);
        yield return transition.WaitForCompletion();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//replace with next level GUI
        transitionImage.DOFade(0f, 2f).SetDelay(0.5f).SetEase(Ease.InSine);
        isRunning = false;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => players.Clear();
}
