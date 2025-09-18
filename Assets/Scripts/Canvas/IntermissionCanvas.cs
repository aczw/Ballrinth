using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntermissionCanvas : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private GameObject dummyPowerUp;

    [Header("Setup")]
    [SerializeField] private Transform powerUpChoices;
    [SerializeField] private IntermissionChoice[] choices;

    private IPowerUp choiceOne;
    private IPowerUp choiceThree;
    private IPowerUp choiceTwo;
    private SelectedChoice selected;

    private void Awake() => Reset();

    private void Reset() => selected = SelectedChoice.None;

    private void Update() {
        if (selected == SelectedChoice.None) return;

        for (var possibleChoice = 0; possibleChoice < 3; ++possibleChoice) {
            choices[possibleChoice].button.interactable = selected != (SelectedChoice)possibleChoice;
        }
    }

    private void OnEnable() {
        var inventoryHasSpace = GameManager.I.inventory.GetFullSlots().Count < 3;

        foreach (var choice in choices) {
            choice.button.interactable = inventoryHasSpace;
        }

        var possiblePowerUps = GameManager.I.GetPossiblePowerUps();
        var numPowerUps = possiblePowerUps.Count;

        choiceOne = possiblePowerUps[Random.Range(0, numPowerUps)];
        choices[0].title.text = choiceOne.GetName();
        choiceTwo = possiblePowerUps[Random.Range(0, numPowerUps)];
        choices[1].title.text = choiceTwo.GetName();
        choiceThree = possiblePowerUps[Random.Range(0, numPowerUps)];
        choices[2].title.text = choiceThree.GetName();
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
            GameManager.I.inventory.Add(choiceOne);
            break;
        case SelectedChoice.Two:
            GameManager.I.inventory.Add(choiceTwo);
            break;
        case SelectedChoice.Three:
            GameManager.I.inventory.Add(choiceThree);
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