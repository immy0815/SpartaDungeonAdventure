using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemSlotPopup : MonoBehaviour
{
    [SerializeField] private UIItemSlot[] itemSlots;
    private Dictionary<ItemData, UIItemSlot> itemDataList;
    
    void Reset()
    {
        itemSlots = GetComponentsInChildren<UIItemSlot>();
    }

    void Awake()
    {
        itemDataList = new Dictionary<ItemData, UIItemSlot>();
    }
    
    void Start()
    {
        CharacterManager.Instance.Player.OnAddItem += AddItem;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemSlots[1].OnClick();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemSlots[2].OnClick();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            itemSlots[3].OnClick();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            itemSlots[4].OnClick();
        }
    }
    
    void AddItem(ItemData data)
    {
        if (itemDataList.ContainsKey(data)) // 만약 이미 슬롯에 있는 아이템이라면
        {
            // 스택형이면서 풀 스택이라면
            if (data.canStack && data.maxStackAmount > itemDataList[data].GetQuantity())
            {
                itemDataList[data].AddQuantity(1);
                return;
            }
            
            // 아니면 버리기
            ThrowItem(data);
        }
        else
        {
            // 아이템 슬롯이 꽉 찬 경우
            if (itemDataList.Count >= itemSlots.Length)
            {
                Debug.Log("아이템을 더 이상 얻을 수 없습니다.");
                ThrowItem(data);
                return;
            }
            
            // 빈 아이템 슬롯이 있는 경우
            itemDataList.Add(data, itemSlots[itemDataList.Count]);
            itemDataList[data].Show(data);
            itemDataList[data].AddListener(() => UseItem(data));
        }
    }

    void ThrowItem(ItemData data)
    {
        // 바닥에 떨구기
        Instantiate(data.dropPrefab, CharacterManager.Instance.Player.dropPosition.position, 
            Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    
    void UseItem(ItemData data)
    {
        if (data == null) return;
        
        itemDataList[data].AddQuantity(-1);
        
        if (itemDataList[data].GetQuantity() <= 0)
        {
            itemDataList.Remove(data);
        }
        
        switch (data.type)
        {
            case ItemType.Consumable:
                for (int i = 0; i < data.consumables.Length; i++)
                {
                    switch (data.consumables[i].type)
                    {
                        case ConsumableType.Speed:
                            StartCoroutine(Booster(data.consumables[i].value));
                            break;
                        case ConsumableType.JumpCount:
                        case ConsumableType.Super:
                            Debug.Log("Not implemented yet.");
                            break;
                    }
                }
                break;
            case ItemType.Equipable:
            case ItemType.Resource:
            default:
                Debug.Log("Not implemented yet.");
                break;
        }
    }
    
    IEnumerator Booster(float addSpeed)
    {
        CharacterManager.Instance.Player.PlayerController.moveSpeed += addSpeed;
        
        yield return new WaitForSeconds(5);
        
        CharacterManager.Instance.Player.PlayerController.moveSpeed -= addSpeed;
    }
}