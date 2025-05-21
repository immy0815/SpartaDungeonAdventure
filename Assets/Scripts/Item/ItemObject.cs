using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
    
    public ItemData GetItemData();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData data;
    
    public string GetInteractPrompt()
    {
        string str = $"<size=64><b>{data.displayName}</b></size>" +
                     $"\n<size=36>{data.description}</size>";
        return str;
    }

    public void OnInteract()
    {
        // CharacterManager.Instance.Player.itemData = data;
        // CharacterManager.Instance.Player.OnAddItem?.Invoke();
        Destroy(gameObject);
    }
    
    public ItemData GetItemData() => data;
}
