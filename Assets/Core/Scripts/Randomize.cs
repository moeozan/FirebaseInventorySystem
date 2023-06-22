using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Randomize
{
    public static InventoryItem AddRightHandItemToInventory(Weapons typeOfWeapon, int attack, int defense)
    {
        ItemTypes type = new(typeOfWeapon);
        InventoryItem temp = new("Moe's " + typeOfWeapon.ToString(), type, attack, defense, "This is a : " + typeOfWeapon.ToString());
        return temp;
    }

    public static InventoryItem AddRLeftHandItemToInventory(Weapons typeOfWeapon, int attack, int defense)
    {
        ItemTypes type = new(typeOfWeapon);
        InventoryItem temp = new("Moe's " + typeOfWeapon.ToString(), type, attack, defense, "This is a : " + typeOfWeapon.ToString());
        return temp;
    }
}
