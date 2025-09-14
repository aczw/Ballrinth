using System;
using UnityEngine;
using UnityEngine.UI;

public class IntermissionCanvas : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private GameObject dummyPowerUp;

    [Header("Setup")]
    [SerializeField] private Transform powerUpChoices;
    [SerializeField] private Button[] choiceButtons;

    private IPowerUp choiceOne;
    private IPowerUp choiceThree;
    private IPowerUp choiceTwo;
    private SelectedChoice selected;

    private void Awake() => Reset();

    private void Reset() => selected = SelectedChoice.None;

    private void Update() {
        if (selected == SelectedChoice.None) return;

        for (var possibleChoice = 0; possibleChoice < 3; ++possibleChoice) {
            choiceButtons[possibleChoice].interactable = selected != (SelectedChoice)possibleChoice;
        }
    }

    private void OnEnable() {
        var inventoryHasSpace = GameManager.I.GetFullSlots().Count < 3;

        foreach (var button in choiceButtons) {
            button.interactable = inventoryHasSpace;
        }

        choiceOne = Instantiate(dummyPowerUp, powerUpChoices).GetComponent<IPowerUp>();
        choiceTwo = Instantiate(dummyPowerUp, powerUpChoices).GetComponent<IPowerUp>();
        choiceThree = Instantiate(dummyPowerUp, powerUpChoices).GetComponent<IPowerUp>();
    }

    private void OnDisable() {
        foreach (Transform child in powerUpChoices) {
            Destroy(child.gameObject);
        }

        Reset();
    }

    public void ChoosePowerUpOne() => selected = SelectedChoice.One;
    public void ChoosePowerUpTwo() => selected = SelectedChoice.Two;
    public void ChoosePowerUpThree() => selected = SelectedChoice.Three;

    public void EnterNextStage() {
        switch (selected) {
        case SelectedChoice.One:
            GameManager.I.AddPowerUpToInventory(choiceOne);
            break;
        case SelectedChoice.Two:
            GameManager.I.AddPowerUpToInventory(choiceTwo);
            break;
        case SelectedChoice.Three:
            GameManager.I.AddPowerUpToInventory(choiceThree);
            break;
        case SelectedChoice.None:
            break;
        default:
            throw new ArgumentOutOfRangeException();
        }

        GameManager.I.EnterNextStage();
    }

    private enum SelectedChoice
    {
        One,
        Two,
        Three,
        None
    }
}