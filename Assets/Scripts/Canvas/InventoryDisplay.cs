using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Toggle slotOne;
    [SerializeField] private Toggle slotTwo;
    [SerializeField] private Toggle slotThree;

    private readonly Array slotEnumValues = Enum.GetValues(typeof(Inventory.Slot));

    private void Update() {
        var fullSlots = GameManager.I.GetFullSlots();

        foreach (Inventory.Slot slotValue in slotEnumValues) {
            var slotDisplay = slotValue switch {
                Inventory.Slot.One => slotOne,
                Inventory.Slot.Two => slotTwo,
                Inventory.Slot.Three => slotThree,
                _ => throw new ArgumentOutOfRangeException(nameof(slotValue), slotValue, null)
            };

            slotDisplay.isOn = fullSlots.Contains(slotValue);
        }
    }
}