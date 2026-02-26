using UnityEngine;
using System;

public class LevelConstants : MonoBehaviour
{
    #region Variables Initialization
    private const string SavedLevelIndexKey = "SavedLevelIndex";
    private const string VibrationEnabledKey = "VibrationEnabled";
    private const string MusicEnabledKey = "MusicEnabled";
    private const string SFXEnabledKey = "SFXEnabled";
    public static event Action OnSettingsChanged;

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
    
    #region Music PlayerPrefs[

    public static void setMusicEnabled(bool isEnabled)
    {
        PlayerPrefs.SetInt(MusicEnabledKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool getMusicEnabled()
    {
        return PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
    }
    #endregion
    
    #region SFX PlayerPrefs[

    public static void setSFXEnabled(bool isEnabled)
    {
        PlayerPrefs.SetInt(SFXEnabledKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool getSFXEnabled()
    {
        return PlayerPrefs.GetInt(SFXEnabledKey, 1) == 1;
    }
    #endregion

    
}
