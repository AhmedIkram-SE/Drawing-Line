using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    #region Variables - Audio Sources
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource = null;
    [SerializeField] private AudioSource musicSource = null;
    #endregion

    #region Variables - Audio Clips
    [Header("SFX Clips")]
    [SerializeField] private AudioClip levelWinClip = null;
    [SerializeField] private AudioClip levelLoseClip = null;
    [SerializeField] private AudioClip buttonPressClip = null;

    [Header("Music Clips")]
    [SerializeField] private AudioClip mainMenuMusic = null;
    #endregion

    #region Initialization
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Common practice for AudioManagers to persist across levels
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Read the saved preference and apply it immediately on startup
        bool isMusicEnabled = LevelConstants.getMusicEnabled();
        bool isSFXEnabled = LevelConstants.getSFXEnabled();
        
        SetMusicMute(!isMusicEnabled);
        SetSFXMute(!isSFXEnabled);
    }
    #endregion

    #region Play Methods (SFX)
    public void PlayLevelWin() => PlaySFX(levelWinClip);
    public void PlayLevelLose() => PlaySFX(levelLoseClip);
    public void PlayButtonPressed() => PlaySFX(buttonPressClip);


    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    #endregion

    #region Play Methods (Music)
    public void PlayMainMenuSound()
    {
        if (mainMenuMusic != null && musicSource != null)
        {
            // Avoid restarting if it's already playing
            if (musicSource.clip == mainMenuMusic && musicSource.isPlaying) return;

            musicSource.clip = mainMenuMusic;
            musicSource.loop = true; // Typically menu music loops
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }
    #endregion

    #region Settings & Muting Logic
    // Cleaned up to ensure clarity on what 'true' means (true = is muted)
    public void SetMasterMute(bool isMuted)
    {
        SetMusicMute(isMuted);
        SetSFXMute(isMuted);
    }

    public void SetMusicMute(bool isMuted)
    {
        if (musicSource != null) musicSource.mute = isMuted;
    }

    public void SetSFXMute(bool isMuted)
    {
        if (sfxSource != null) sfxSource.mute = isMuted;
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null) sfxSource.volume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null) musicSource.volume = Mathf.Clamp01(volume);
    }
    #endregion
}