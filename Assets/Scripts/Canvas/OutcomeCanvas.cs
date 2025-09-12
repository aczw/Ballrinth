using TMPro;
using UnityEngine;

public class OutcomeCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text result;

    private void OnEnable() {
        var run = GameManager.I.GetRunState();

        if (run.state != GameManager.State.End) {
            Debug.LogError($"Error! Outcome Canvas has been activated even though run state is {run.state}!");
            return;
        }

        var maxStagesEscaped = GameManager.I.GetMaxStagesEscaped();
        Debug.Log(run.won
                      ? $"YOU WON THE GAME WITH {maxStagesEscaped} STAGES CLEARED!"
                      : $"YOU LOST! NUM STAGES ESCAPED: {run.stage}, HIGH SCORE: {maxStagesEscaped}");

        result.text = run.won ? "You won!" : "You lost!";
    }
}