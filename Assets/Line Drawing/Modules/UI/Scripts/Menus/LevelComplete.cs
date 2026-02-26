using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public void OnPressNextButton()
    {
        GameManager.instance.ProceedToNextLevel();
        AudioManager.Instance.PlayButtonPressed();
    }
}
