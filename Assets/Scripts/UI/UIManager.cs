using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get 
        {
            if(instance == null)
            {
                instance = new GameObject("UIManager").AddComponent<UIManager>();
            }
            return instance; 
        }
    }
    
    UICanvasGameScene canvasGameScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(instance != this)
            {
                Debug.LogError("UIManager instance already exists! Destroy this.");
                Destroy(this.gameObject);
            }
        }
        
        canvasGameScene = GetComponentInChildren<UICanvasGameScene>();
    }

    void Start()
    {
        canvasGameScene.Initialization();
    }

    public void OpenPrompt(string text) => canvasGameScene.OpenPrompt(text);
    public void ClosePrompt() => canvasGameScene.ClosePrompt();
}
