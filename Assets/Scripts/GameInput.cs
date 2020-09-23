using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{
    [SerializeField] Camera _mainCamera = null;
    [SerializeField] Camera _gameoverCamera = null;
    [SerializeField] Camera _winCamera = null;
    private void Start()
    {
        _mainCamera.enabled = true;
        _gameoverCamera.enabled = false;
         _winCamera.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReloadLevel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
  
  
    public void ReloadLevel()
    {
        int activeSceneIndex =
            SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        _mainCamera.enabled = true;
        _gameoverCamera.enabled = false;
        _winCamera.enabled = false;
    }
    public void GameOver()
    {
        _mainCamera.enabled = false;
        _gameoverCamera.enabled = true;
    }
    public void Win()
    {
        _mainCamera.enabled = false;
        _winCamera.enabled = true;
    }
}
