using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;
    public static CharacterManager Instance
    {
        get 
        {
            if(instance == null)
            {
                Debug.LogError("CharacterManager is NULL. Make sure it's in the scene.");
            }
            return instance; 
        }
    }   
    
    private Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
    
    private void Awake()
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
                Debug.LogError("CharacterManager instance already exists! Destroy this.");
                Destroy(this.gameObject);
            }
        }
    }
}
