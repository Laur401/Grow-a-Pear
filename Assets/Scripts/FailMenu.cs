using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailMenu : MonoBehaviour
{
    [SerializeField] private GameObject failMenu;
    [SerializeField] private GameObject failMenuDefaultSelect;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private Image backgroundImage;
    private AudioSource audioSource;
    private Vector3 origin;
    
    private void Start()
    {
        SceneManager.sceneLoaded+=OnSceneLoaded;
        audioSource = GetComponent<AudioSource>();
        failMenu.SetActive(false);
        var color = backgroundImage.color;
        color.a = 0f;
        backgroundImage.color = color;
        SetOrigin();
    }
    public IEnumerator CallFailMenu()
    {
        FindFirstObjectByType<PauseMenu>().SuspendGameState();
        SimpleAudioManager.Manager.instance.StopSong(0.5f);
        audioSource.PlayOneShot(failSound);
        yield return new WaitForSeconds(0.5f);
        failMenu.SetActive(true);
        backgroundImage.DOFade(0.45f, 0.9f);
        EventSystem.current.SetSelectedGameObject(failMenuDefaultSelect);
        failMenu.transform.DOLocalMoveY(0f, 1f, false);

    }

    void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        //SetOrigin();
    }

    void SetOrigin() => origin = failMenu.transform.position;
    
    public void RestartLevel()
    {
        failMenu.transform.position = origin;
        failMenu.SetActive(false);
        var color = backgroundImage.color;
        color.a = 0f;
        backgroundImage.color = color;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
