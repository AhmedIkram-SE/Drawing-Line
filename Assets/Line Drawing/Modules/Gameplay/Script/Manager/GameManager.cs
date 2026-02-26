using System;
using System.Collections.Generic;
using UnityEngine;

#region Level Configuration

[System.Serializable]
public class LevelShapeConfiguration
{
    public string levelName = "New Level";
    public List<Vector2> shapePoints = new List<Vector2>();
    public float lineThickness = 0.3f;
}
#endregion

public enum GameState
{
    Gameplay,
    Settings,
    LevelComplete
}

public class GameManager : MonoBehaviour
{   
    #region Serialized Fields
    public static GameManager instance;

    [Header("UI References")]
    [SerializeField] private UIManager uiManager = null;
    
    [Header("Level System")]
    [SerializeField] private LevelShapeConfiguration[] levels = null; 
    [SerializeField] private LevelManager levelManager = null;
    #endregion
    
    #region State Variables
    private GameState _currentState = GameState.Gameplay;
    private int _currentLevelIndex = 0;
    private bool _levelEnded = false;
    #endregion
    
    #region Unty Methods

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _currentLevelIndex = LevelConstants.getLevelIndex();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(GameState.Gameplay);
    }

    #endregion

    #region State Methods
    public void ChangeState(GameState newState)
    {
        _currentState = newState;
        Debug.Log($"Draw Line State:{_currentState}");

        if (uiManager != null)
        {
            uiManager.OnGameStateChanged(_currentState);
        }

        switch (_currentState)
        {
            case GameState.Gameplay:
                ExecuteStartGame();
                break;
            case GameState.Settings:
                ExecuteSettings();
                break;
            case GameState.LevelComplete:
                ExecuteLevelComplete();
                break;
        }
    }

    private void ExecuteStartGame()
    {
        _levelEnded = false;
        Time.timeScale = 1;
        _currentLevelIndex = LevelConstants.getLevelIndex();

        if (levels!=null && _currentLevelIndex < levels.Length)
        {
            Debug.Log($"Generating Level: {levels[_currentLevelIndex].levelName}");
            levelManager.GenerateLevelShape(levels[_currentLevelIndex]); // Level manager implementation
        }
        AudioManager.Instance?.PlayMainMenuSound();
    }

    private void ExecuteSettings()
    {
        Debug.Log("Settings Menu opened");
    }

    private void ExecuteLevelComplete()
    {
        _levelEnded = true;
        levelManager.ClearLevel();
        AudioManager.Instance.PlayLevelWin();

    }
    #endregion

    #region API Functions

    public void ProceedToNextLevel() // To be called by the nextlevel button
    {
        _currentLevelIndex++;
        LevelConstants.setLevelIndex(_currentLevelIndex);
        if (_currentLevelIndex >= levels.Length)
        { Debug.Log("All levels completed! Restarting from the first level.");
            LevelConstants.setLevelIndex(0);
        }
        ChangeState(GameState.Gameplay);
    }
    #endregion

}
