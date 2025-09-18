using System;
using System.Collections.Generic;
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

    public void Add(IPowerUp powerUp) {
        if (slotOne == null) {
            slotOne = powerUp;
        }
        else if (slotTwo == null) {
            slotTwo = powerUp;
        }
        else if (slotThree == null) {
            slotThree = powerUp;
        }
    }

    public void TryUse(Slot slot) {
        ref var powerUp = ref slotOne;

        switch (slot) {
        case Slot.One:
            break;
        case Slot.Two:
            powerUp = ref slotTwo;
            break;
        case Slot.Three:
            powerUp = ref slotThree;
            break;
        default:
            throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
        }

        if (powerUp != null) {
            powerUp.Activate();
            powerUp = null;
        }
    }

    public HashSet<Slot> GetFullSlots() {
        var fullSlots = new HashSet<Slot>();

        if (slotOne != null) fullSlots.Add(Slot.One);
        if (slotTwo != null) fullSlots.Add(Slot.Two);
        if (slotThree != null) fullSlots.Add(Slot.Three);

        return fullSlots;
    }

    public IPowerUp Get(Slot slot) {
        return slot switch {
            Slot.One => slotOne,
            Slot.Two => slotTwo,
            Slot.Three => slotThree,
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };
    }

    public void Clear() {
        slotOne = null;
        slotTwo = null;
        slotThree = null;
    }
}