using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    [SerializeField] InputActionAsset inputActionAsset;
    private List<GameObject> players=new List<GameObject>();
    private InputActionMap playerInputMap;
    private InputActionMap inputMap;
    private InputAction pause;
    [SerializeField] private GameObject mainMenuDefaultSelect;

    private static PauseMenu instance = null;
    void Awake() //TODO: Start existence in Main Menu, but activate only in a level
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded+=OnSceneLoaded;
            return;
        }
        Destroy(gameObject);
    }
    void Start()
    {
        FetchPlayers();
        playerInputMap = inputActionAsset.FindActionMap("Player1");
        inputMap = inputActionAsset.FindActionMap("UI");
        pause = playerInputMap.FindAction("Pause");
        pause.performed += OnInput;
        pauseMenu.SetActive(false);
    }

    
    void OnInput(InputAction.CallbackContext obj)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenuDefaultSelect);
        SuspendGameState();
    }

    public void SuspendGameState()
    {
        foreach (GameObject player in players)
            EnableDisablePlayers(false);
        //Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        foreach (GameObject player in players)
            EnableDisablePlayers(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        gameObject.GetComponent<KeepAlive>().enabled=false;
        SceneManager.LoadScene("Scenes/Main Menu");
        Destroy(gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        players.Clear();
        FetchPlayers();
    }

    void FetchPlayers()
    {
        PlayerMovement1[] playersFind=FindObjectsByType<PlayerMovement1>(FindObjectsSortMode.None);
        foreach (PlayerMovement1 p in playersFind)
            players.Add(p.gameObject);
    }

    void EnableDisablePlayers(bool value)
    {
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerMovement1>().enabled = value;
            //...add other scripts if needed later
        }
    }
}
