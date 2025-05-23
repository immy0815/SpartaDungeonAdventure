using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICanvasGameScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpPrompt;
    

    void Reset()
    {
        tmpPrompt = transform.Find("Tmp_Prompt")?.GetComponent<TextMeshProUGUI>();
    }

    public void Initialization()
    {
        ClosePrompt();
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
}
