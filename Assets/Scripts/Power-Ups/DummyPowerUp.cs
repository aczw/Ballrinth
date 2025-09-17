using UnityEngine;

public class DummyPowerUp : MonoBehaviour, IPowerUp
{
    public void Activate() => Debug.Log("Dummy power-up activated!");
    public string GetName() => "Dummy (not real)";
}