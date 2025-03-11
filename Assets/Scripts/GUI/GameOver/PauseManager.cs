using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;  

    private void Start()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {        
        if (_gameOverPanel.activeSelf)
        {
            return;
        }

        Time.timeScale = 1;
    }
}
