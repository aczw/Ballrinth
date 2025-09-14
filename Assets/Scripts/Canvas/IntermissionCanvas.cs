using UnityEngine;

public class IntermissionCanvas : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private GameObject dummyPowerUp;

    [Header("Setup")]
    [SerializeField] private Transform powerUpChoices;

    private IPowerUp choiceOne;
    private IPowerUp choiceThree;
    private IPowerUp choiceTwo;

    private void OnEnable() {
        choiceOne = Instantiate(dummyPowerUp, powerUpChoices).GetComponent<IPowerUp>();
        choiceTwo = Instantiate(dummyPowerUp, powerUpChoices).GetComponent<IPowerUp>();
        choiceThree = Instantiate(dummyPowerUp, powerUpChoices).GetComponent<IPowerUp>();
    }

    private void OnDisable() {
        foreach (Transform child in powerUpChoices) {
            Destroy(child.gameObject);
        }
    }

    public void ChoosePowerUpOne() => GameManager.I.AddPowerUpToInventory(choiceOne);
    public void ChoosePowerUpTwo() => GameManager.I.AddPowerUpToInventory(choiceTwo);
    public void ChoosePowerUpThree() => GameManager.I.AddPowerUpToInventory(choiceThree);
}