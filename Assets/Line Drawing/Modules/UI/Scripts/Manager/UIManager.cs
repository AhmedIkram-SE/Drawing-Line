using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Menu Prefabs
    [Header("Menu Prefabs")]
    [SerializeField] GameObject gameplayMenuPrefab = null;
    [SerializeField] GameObject settingsMenuPrefab = null;
    [SerializeField] GameObject levelcompleteMenuPrefab = null;
    #endregion
    
    #region Private Variables
    private GameObject _currentMenu = null;
    private TextMeshProUGUI _currentLevelText = null;
    #endregion
    
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
            
            RectTransform rect = _currentMenu.GetComponent<RectTransform>();
            if (rect != null)
            {
                // FORCE SCALE: This fixes the '0 scale' issue
                rect.localScale = Vector3.one; 
            
                // CENTER & STRETCH: This fixes the 'misaligned icons' issue
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            
                // Ensure it is at the center of the Canvas
                rect.localPosition = Vector3.zero;
            }
            
            // If it's Gameplay, find the Level Text (e.g., "Level 6")
            if (menuPrefab == gameplayMenuPrefab)
            {
                _currentLevelText = gameplayMenuPrefab.GetComponent<TextMeshProUGUI>();
            }
        }
        
    }
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
