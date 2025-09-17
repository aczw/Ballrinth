using UnityEngine;

public class IncreaseTimePowerUp : MonoBehaviour, IPowerUp
{
    [Header("Configuration")]
    [SerializeField] private float timeAdded;

    public void Activate() => GameManager.I.timer.AddCurrentTime(timeAdded);
    public string GetName() => "Add more time";
}