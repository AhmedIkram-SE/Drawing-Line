using UnityEngine;

public class GameplayMenuController : MonoBehaviour
{
    public void OnSettingsButtonPressed()
    {
        GameManager.instance.ChangeState(GameState.Settings);
        AudioManager.Instance.PlayButtonPressed();
    }
}
