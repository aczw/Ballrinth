using System.Collections;
using UnityEngine;

public class StopTimerPowerUp : MonoBehaviour, IPowerUp
{
    [Header("Configuration")]
    [SerializeField] private float duration;

    public void Activate() => StartCoroutine(ApplyStop());

    public string GetName() => "Stop timer";

    private IEnumerator ApplyStop() {
        GameManager.I.timer.Pause();

        yield return new WaitForSeconds(duration);

        GameManager.I.timer.Resume();
    }
}