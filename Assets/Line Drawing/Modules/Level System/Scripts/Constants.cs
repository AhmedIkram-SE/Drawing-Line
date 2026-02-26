using UnityEngine;
using System;

public class LevelConstants : MonoBehaviour
{
    #region Variables Initialization
    private const string SavedLevelIndexKey = "SavedLevelIndex";
    private const string VibrationEnabledKey = "VibrationEnabled";
    private const string SoundEnabledKey = "SoundEnabled";

    public static event Action OnSettingsChanged;
    public static event Action OnCoinsChanged;     // Simple event that UI scripts can subscribe to

    #endregion

    #region PlayerPrefs Functions
    public static void setLevelIndex(int levelIndex)
    {
        PlayerPrefs.SetInt(SavedLevelIndexKey, levelIndex);
        PlayerPrefs.Save();
    }
    public static int getLevelIndex()
    {
        return PlayerPrefs.GetInt(SavedLevelIndexKey, 0);
    }
    #endregion


    #region Vibration PlayerPrefs
    public static void setVibrationEnabled(bool isEnabled)
    {
        // We store bool as int (1 for true, 0 for false)
        PlayerPrefs.SetInt(VibrationEnabledKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

        // Broadcast the change in case a UI icon needs to switch (e.g., mute icon)
        OnSettingsChanged?.Invoke();
    }

    public static bool getVibrationEnabled()
    {
        // Default to 1 (true) so the game has vibration the first time it's played
        return PlayerPrefs.GetInt(VibrationEnabledKey, 1) == 1;
    }

    #endregion

    #region Sound PlayerPrefs

    public static void setSoundEnabled(bool isEnabled)
    {
        PlayerPrefs.SetInt(SoundEnabledKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        OnSettingsChanged?.Invoke();
    }
    public static bool getSoundEnabled()
    {
        return PlayerPrefs.GetInt(SoundEnabledKey, 1) == 1;
    }
    #endregion

    
}
