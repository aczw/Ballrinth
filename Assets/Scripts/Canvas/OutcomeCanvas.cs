using TMPro;
using UnityEngine;

public class OutcomeCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text result;

    private void OnEnable() {
        var state = GameManager.I.GetGameState();

        if (state.status != GameState.Status.End) {
            Debug.LogError($"Error! Outcome Canvas has been activated even though run state is {state.status}!");
            return;
        }

        var maxStagesEscaped = state.maxStagesEscaped;
        var run = GameManager.I.GetRunState();

        Debug.Log(run.won
                      ? $"YOU WON THE GAME WITH {maxStagesEscaped} STAGES CLEARED!"
                      : $"YOU LOST! NUM STAGES ESCAPED: {run.stage}, HIGH SCORE: {maxStagesEscaped}");

        result.text = run.won ? "You won!" : "You lost!";
    }
}