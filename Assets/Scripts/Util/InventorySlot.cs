using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private TMP_Text title;

    public void SetIsOn(bool status) => toggle.isOn = status;
    public void SetNewTitle(string newText) => title.text = newText;
}