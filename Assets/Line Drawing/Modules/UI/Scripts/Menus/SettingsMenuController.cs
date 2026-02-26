using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    #region Togles Serialzed References
    [Header("Togles Serialzed References")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle vibrateToggle;
    #endregion

    private void OnEnable()
    {
        if(musicToggle!=null)
        {
            musicToggle.isOn = LevelConstants.getMusicEnabled();
        }
        if(sfxToggle!=null)
        {
            sfxToggle.isOn = LevelConstants.getSFXEnabled();
        }

        if (vibrateToggle != null)
        {
            vibrateToggle.isOn = LevelConstants.getVibrationEnabled();
        }
    }

    public void OnCrossButtonPressed()
    {
        GameManager.instance.ChangeState(GameState.Gameplay);
        AudioManager.Instance.PlayButtonPressed();
    }

    public void OnMusicToggle(bool isMusicOn)
    {
        LevelConstants.setMusicEnabled(isMusicOn);
        if (AudioManager.Instance!= null)
        {
            AudioManager.Instance.SetMusicMute(!isMusicOn);
        }
    }

    public void OnSFXToggle(bool isSfxOn)
    {
        LevelConstants.setSFXEnabled(isSfxOn);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXMute(!isSfxOn);
        }
    }
    
    public void OnVibrateToggle(bool isVibrateOn) 
    {
        LevelConstants.setVibrationEnabled(isVibrateOn);
    }
    
    
}
