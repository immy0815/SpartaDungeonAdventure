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
    public bool isItemUsing;
    public float curHealth;
    public float maxHealth;

    private Dictionary<Attackable, Coroutine> damageCoroutines;
    
    private void Reset()
    {
        dropPosition = transform.Find("DropPosition")?.GetComponent<Transform>();
        maxHealth = 100;
        curHealth = maxHealth;
    }

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerInputSystem = GetComponent<PlayerInputSystem>();


        damageCoroutines = new Dictionary<Attackable, Coroutine>();
        CharacterManager.Instance.Player = this;
    }

    // Status <= 
    IEnumerator TakeDamage(float dmg)
    {
        while (true)
        {
            curHealth -= dmg;
            float healthRatio = Mathf.InverseLerp(0f, maxHealth, curHealth);
            UIManager.Instance.OnHealthUI?.Invoke(healthRatio);

            if (curHealth <= 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attackable"))
        {
            Attackable attackObj = other.gameObject.GetComponent<Attackable>();
            damageCoroutines.Add(attackObj, StartCoroutine(TakeDamage(attackObj.power)));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Attackable"))
        {
            Attackable attackObj = other.gameObject.GetComponent<Attackable>();
            StopCoroutine(damageCoroutines[attackObj]);
            damageCoroutines.Remove(attackObj);
        }
    }
}
