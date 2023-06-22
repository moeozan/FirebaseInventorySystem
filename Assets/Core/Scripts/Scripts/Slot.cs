using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public bool IsEmpty { get; set; }
    public InventoryItem Item { get; set; }
    public int Index { get; set; }
    public Sprite emptySlotSprite;
    [Header("Children")]
    public Image choosen;
    public Transform informationPanelTransform;

    private void Awake()
    {
        choosen.gameObject.SetActive(false);
    }

    public void ItemInitialize()
    {
        GetComponent<Image>().sprite = GetComponentInParent<InventoryController>().FindItemSprite(Item.ItemName);
        IsEmpty = false;
    }

    public void Empty()
    {
        GetComponent<Image>().sprite = emptySlotSprite;
    }
}
