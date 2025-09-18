using System;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private InventorySlot slotOne;
    [SerializeField] private InventorySlot slotTwo;
    [SerializeField] private InventorySlot slotThree;

    private readonly Array slotEnumValues = Enum.GetValues(typeof(Inventory.Slot));

    private void Update() {
        var fullSlots = GameManager.I.inventory.GetFullSlots();

        foreach (Inventory.Slot slotValue in slotEnumValues) {
            var slot = slotValue switch {
                Inventory.Slot.One => slotOne,
                Inventory.Slot.Two => slotTwo,
                Inventory.Slot.Three => slotThree,
                _ => throw new ArgumentOutOfRangeException(nameof(slotValue), slotValue, null)
            };

            slot.SetIsOn(fullSlots.Contains(slotValue));

            var powerUp = GameManager.I.inventory.Get(slotValue);
            slot.SetNewTitle(powerUp == null ? "<empty>" : powerUp.GetName());
        }
    }

    private void OnEnable() {
        slotOne.SetIsOn(false);
        slotTwo.SetIsOn(false);
        slotThree.SetIsOn(false);
    }
}