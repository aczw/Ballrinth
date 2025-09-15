using UnityEngine;

public class IncreaseTimePowerUp : MonoBehaviour, IPowerUp
{
    public void Activate() => GameManager.I.AddRemainingTime(20f);
}