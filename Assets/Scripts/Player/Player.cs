using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
    public PlayerInputSystem PlayerInputSystem { get; private set; }
    // public ItemData itemData;
    public Action<ItemData> OnAddItem;
    public Transform dropPosition;

    private void Reset()
    {
        dropPosition = transform.Find("DropPosition")?.GetComponent<Transform>();
    }

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        PlayerController = GetComponent<PlayerController>();
        PlayerInputSystem = GetComponent<PlayerInputSystem>();
    }
}
