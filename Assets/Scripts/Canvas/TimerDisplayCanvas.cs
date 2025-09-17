using TMPro;
using UnityEngine;

public class TimerDisplayCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text currentTime;

    private void Update() {
        if (GameManager.I.GetGameState().status != GameState.Status.InGame) return;

        var time = GameManager.I.timer.GetCurrentTime();
        currentTime.text = time.ToString("F2");
    }
}