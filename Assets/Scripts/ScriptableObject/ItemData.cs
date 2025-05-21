using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,   // 장착 가능 아이템
    Consumable,  // 소비 가능 아이템
    Resource,    // 자원 아이템 (퀘스트 같은 곳에도 사용 가능)
}

public enum ConsumableType
{
    Speed,
    JumpCount,
    Super
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;
    
    [Header("Stacking")] 
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")] 
    public ItemDataConsumable[] consumables;
}
