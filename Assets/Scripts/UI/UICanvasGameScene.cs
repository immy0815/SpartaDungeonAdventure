using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasGameScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpPrompt;
    [SerializeField] private Image imgHealth;
    
    //OnHealthUI

    void Reset()
    {
        tmpPrompt = transform.Find("Tmp_Prompt")?.GetComponent<TextMeshProUGUI>();
        imgHealth = transform.Find("Group_Health/Img_HealthBar")?.GetComponent<Image>();
    }

    public void Initialization()
    {
        UIManager.Instance.OnHealthUI -= UpdateHealthBar;
        UIManager.Instance.OnHealthUI += UpdateHealthBar;
        ClosePrompt();
        imgHealth.fillAmount = 1;
    }
    
    public void OpenPrompt(string text)
    {
        tmpPrompt.gameObject.SetActive(true);
        tmpPrompt.text = text;
    }

    public void ClosePrompt()
    {
        tmpPrompt.gameObject.SetActive(false);
    }

    public void UpdateHealthBar(float value)
    {
        imgHealth.fillAmount = value;
    }
    
}
