using Firebase.Firestore;
using System;
using System.Collections.Generic;

[FirestoreData]
public class ItemTypes
{
    [FirestoreProperty] public int WeaponIndex { get; set; }
    public ItemTypes() { }
    public ItemTypes(Weapons weapon)
    {
        WeaponIndex = (int)weapon;
    }

    public Weapons WeaponReturner(int index)
    {
        Weapons w = (Weapons)index;
        return w;
    }
}



[FirestoreData]
public enum Weapons
{
    Axe,
    Sword,
    Bow,
    Shield,
    Knot
}
/*
[FirestoreData]
public class ItemTypes
{
    [FirestoreProperty] public RightHand Right { get; set; }
    [FirestoreProperty] public LeftHand Left { get; set; }
    [FirestoreProperty] public int ItemId { get; set; }

    public ItemTypes() { }

    public ItemTypes(RightHand right)
    {
        Right = right;
        Left = LeftHand.NULL;
    }
    public ItemTypes(LeftHand left)
    {
        Left = left;
        Right = RightHand.NULL;
    }
}

[FirestoreData]
public enum LeftHand
{
    NULL,
    Axe,
    Sword,
    Bow,
}
[FirestoreData]
public enum RightHand
{
    NULL,
    Shield,
    Knot,
}
*/