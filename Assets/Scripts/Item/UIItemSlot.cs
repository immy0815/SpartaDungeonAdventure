using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] private Image imgItemIcon;
    [SerializeField] private TextMeshProUGUI tmpQuantity;
    [SerializeField] private Button btnItemSlot;
    
    [SerializeField] int quantity;

    private void Reset()
    {
        btnItemSlot = GetComponent<Button>();
        imgItemIcon = transform.Find("Img_Icon")?.GetComponent<Image>();
        tmpQuantity = transform.Find("Tmp_Quantity")?.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        Initialization();
    }
    
    void Initialization()
    {
        itemData = null;
        imgItemIcon.gameObject.SetActive(false);
        tmpQuantity.text = string.Empty;
        btnItemSlot.onClick.RemoveAllListeners();
    }

    public void Show(ItemData data)
    {
        itemData = data;
        imgItemIcon.gameObject.SetActive(true);
        imgItemIcon.sprite = itemData.icon;
        quantity = 1;
        tmpQuantity.text = quantity.ToString();
    }
    
    public void AddQuantity(int num)
    {   
        quantity += num;

        if (quantity <= 0)
        {
            Initialization();
            return;
        }

        UpdateUIText();
    }

    void UpdateUIText()
    {
        tmpQuantity.text = quantity.ToString();
    }

    public void AddListener(UnityAction callback)
    {
        btnItemSlot.onClick.RemoveAllListeners();
        btnItemSlot.onClick.AddListener(callback);
    }

    public void OnClick()
    {
        btnItemSlot.onClick.Invoke();
    }
    
    public int GetQuantity() => quantity;
    
    public ItemData GetItemData() => itemData;
}
