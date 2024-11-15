using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthUI : MonoBehaviour
{
    //TODO: Add level fail UI
    TextMeshProUGUI healthText;
    private int levelHealth;
    private int currentHealth;
    private void Awake() => SceneManager.sceneLoaded+=OnSceneLoaded;
    private void Start()
    {
        healthText=GetComponent<TextMeshProUGUI>();
        FetchHealth();
    }
    
    private void UpdateText()
    {
        if (currentHealth <= 0)
            StartCoroutine(FindFirstObjectByType<FailMenu>().CallFailMenu());
        healthText.text = currentHealth.ToString();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FetchHealth();
    }

    public void RemoveLife()
    {
        currentHealth--;
        UpdateText();
    }

    public void AddLife()
    {
        currentHealth++;
        UpdateText();
    }

    private void FetchHealth()
    {
        Variables localVariables = FindFirstObjectByType<Variables>();
        levelHealth = localVariables.playerLives;
        currentHealth = levelHealth;
        UpdateText();
    }

    
}
