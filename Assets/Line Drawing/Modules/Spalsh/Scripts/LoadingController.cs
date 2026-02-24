using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    #region Variables
    [Header("UI References")]
    [SerializeField] private Image loadingBarFill = null;
    [SerializeField] private TextMeshProUGUI loadingText = null;

    [Header("Settings")]
    [SerializeField] private string gameplaySceneName = "Gameplay Scene";
    [SerializeField] private float loadSpeed = 0.5f; // Adjust this to control "smoothness"
    
    private float targetProgress = 0f;
    private float currentDisplayedProgress = 0f;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Initializing variables to default states as per standards
        if (loadingBarFill != null) loadingBarFill.fillAmount = 0;
        if (loadingText != null) loadingText.text = "0 %";
        
        StartCoroutine(LoadSceneCoroutine());
    }
    #endregion

    #region Loading Logic
    private IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameplaySceneName);
        operation.allowSceneActivation = false;

        while (currentDisplayedProgress < 1.0f)
        {
            // Get the actual progress from Unity (0.0 to 0.9)
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Smoothly move the visual progress toward the target progress
            // This prevents the "jump" to 100%
            currentDisplayedProgress = Mathf.MoveTowards(
                currentDisplayedProgress, 
                targetProgress, 
                loadSpeed * Time.deltaTime
            );

            UpdateLoadingUI(currentDisplayedProgress);

            // Once the bar reaches 100% visually and Unity is ready
            if (currentDisplayedProgress >= 1.0f && operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.2f); // Short buffer for visual polish
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void UpdateLoadingUI(float progress)
    {
        if (loadingBarFill != null) 
        {
            loadingBarFill.fillAmount = progress;
        }
        
        if (loadingText != null)
        {
            // FloorToInt ensures we don't see decimals like 50.444%
            int percent = Mathf.FloorToInt(progress * 100f);
            loadingText.text = percent+"%";
        }
    }
    #endregion
}