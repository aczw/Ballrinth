using System;
using JetBrains.Annotations;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum Slot
    {
        One = 1,
        Two,
        Three
    }

    [CanBeNull] private IPowerUp slotOne;
    [CanBeNull] private IPowerUp slotThree;
    [CanBeNull] private IPowerUp slotTwo;

    public void TryUse(Slot slot) {
        switch (slot) {
        case Slot.One:
            slotOne?.Activate();
            break;
        case Slot.Two:
            slotTwo?.Activate();
            break;
        case Slot.Three:
            slotThree?.Activate();
            break;
        default:
            throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
        }
    }
}