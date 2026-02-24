using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    public void OnCrossButtonPressed()
    {
        GameManager.instance.ChangeState(GameState.Gameplay);
    }
}
