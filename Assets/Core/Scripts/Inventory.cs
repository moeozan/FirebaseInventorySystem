using Firebase.Firestore;
using UnityEngine.UI;

[FirestoreData]
public class Inventory
{
    [FirestoreProperty] public InventorySlot[] Slots { get; set; } = new InventorySlot[]
        {
            new InventorySlot(),new InventorySlot(),new InventorySlot(),new InventorySlot(),
            new InventorySlot(),new InventorySlot(),new InventorySlot(),new InventorySlot(),
            new InventorySlot(),new InventorySlot(),new InventorySlot(),new InventorySlot(),
            new InventorySlot(),new InventorySlot(),new InventorySlot(),new InventorySlot(),
            new InventorySlot(),new InventorySlot(),new InventorySlot(),new InventorySlot()
        };

    public Inventory() { }
}

[FirestoreData]
public class InventorySlot
{
    [FirestoreProperty] public InventoryItem Item { get; set; } = new InventoryItem();

    public InventorySlot() { }
}

[FirestoreData]
public class InventoryItem
{
    [FirestoreProperty] public string ItemName { get; set; }
    [FirestoreProperty] public ItemTypes ItemType { get; set; }
    [FirestoreProperty] public int Attack { get; set; }
    [FirestoreProperty] public int Defense { get; set; }
    [FirestoreProperty] public string Description { get; set; }



    public InventoryItem ( ) { }

    public InventoryItem (string itemName, ItemTypes weaponType, int attack, int defense, string description)
    {
        ItemName = itemName;
        ItemType = weaponType;
        Attack = attack;
        Defense = defense;
        Description = description;
    }
}