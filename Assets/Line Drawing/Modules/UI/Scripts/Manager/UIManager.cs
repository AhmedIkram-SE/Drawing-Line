using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Menu Prefabs
    [Header("Menu Prefabs")]
    [SerializeField] private GameObject gameplayMenuPrefab = null;
    [SerializeField] private GameObject settingsMenuPrefab = null;
    [SerializeField] private GameObject levelcompleteMenuPrefab = null;
    #endregion

    #region Helper References
    [SerializeField] private GameObject DrawingController = null;
    #endregion

    #region Private Variables
    private GameObject _currentMenu = null;
    private TextMeshProUGUI _currentLevelText = null;
    private Canvas _canvas = null;
    #endregion

    private void Start()
    {
         _canvas = this.GetComponent<Canvas>(); // Getting canvas reference for render mode adjustments
    }

    #region State Management
    public void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Gameplay:
                SwitchMenu(gameplayMenuPrefab);
                break;
            case GameState.Settings:
                SwitchMenu(settingsMenuPrefab);
                break;
            case GameState.LevelComplete:
                SwitchMenu(levelcompleteMenuPrefab);
                break;
        }
    }

    private void SwitchMenu(GameObject menuPrefab)
    {
        if (_currentMenu != null)
        {
            Destroy(_currentMenu);
            _currentLevelText = null;
        }

        if (menuPrefab != null)
        {
            _currentMenu = Instantiate(menuPrefab,transform);
            
            // If it's Gameplay, find the Level Text (e.g., "Level 6")
            if (menuPrefab == gameplayMenuPrefab)
            {
                Transform hudRoot = _currentMenu.transform.Find("Panel_GamePlay");
                if(hudRoot!= null)
                {
                    Debug.Log("Found Panel_GamePlay in Gameplay Menu Prefab");
                    _currentLevelText = hudRoot.Find("Text_LevelCount")?.GetComponent<TextMeshProUGUI>();
                    _currentLevelText.text = $"Level {LevelConstants.getLevelIndex() + 1}"; // Display current level (1-based index)
                }

                //  _currentLevelText.text = $"Level {LevelConstants.getLevelIndex() + 1}"; // Display current level (1-based index)
                if (_canvas != null)
                    _canvas.renderMode = RenderMode.ScreenSpaceCamera; // Ensure gameplay menu is rendered in world space
                if (DrawingController != null)
                    DrawingController.SetActive(true); // Enable drawing when in gameplay
            }
            if(menuPrefab == settingsMenuPrefab)
            {
                if (_canvas != null)
                    _canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Ensure settings menu is on top
                if (DrawingController != null)
                    DrawingController.SetActive(false); // Disable drawing when in settings
            }
            }
        
    }
    #endregion
    
}
