using System.Collections;
using UnityEngine;

public class SlowDownTimerPowerUp : MonoBehaviour, IPowerUp
{
    [Header("Configuration")]
    [SerializeField] private float duration;
    [SerializeField] private float speedMultiplier;

    public void Activate() => StartCoroutine(ApplySlowDown());
    public string GetName() => "Slow down time";

    private IEnumerator ApplySlowDown() {
        GameManager.I.timer.SetSpeedMultiplier(speedMultiplier);

        yield return new WaitForSeconds(duration);

        GameManager.I.timer.SetSpeedMultiplier(1f);
    }
}