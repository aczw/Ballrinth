using TMPro;
using UnityEngine;

public class OutcomeCanvas : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private TMP_Text result;
    [SerializeField] private TMP_Text yourScore;
    [SerializeField] private TMP_Text highScore;

    private void OnEnable() {
        var state = GameManager.I.GetGameState();

        if (state.status != GameState.Status.End) {
            Debug.LogError($"Error! Outcome Canvas has been activated even though run state is {state.status}!");
            return;
        }

        var run = GameManager.I.GetRunState();

        yourScore.text = run.stage.ToString();
        highScore.text = state.maxStagesEscaped.ToString();
        result.text = run.won ? "You won!" : "You lost!";
    }
}