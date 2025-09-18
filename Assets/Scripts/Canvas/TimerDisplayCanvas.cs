using TMPro;
using UnityEngine;

public class TimerDisplayCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text currentTime;
    [SerializeField] private TMP_Text currentScore;

    private void Update() {
        if (GameManager.I.GetGameState().status != GameState.Status.InGame) return;

        var time = GameManager.I.timer.GetCurrentTime();
        var run = GameManager.I.GetRunState();

        currentTime.text = time.ToString("F2");
        currentScore.text = run.stage.ToString();
    }
}