using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActionType
{
    Move,
    Look,
    Jump,
    Inventory,
    Interact
}

public class PlayerInputSystem : MonoBehaviour
{
    private InputAction moveAction, lookAction, jumpAction, inventoryAction, interactAction;

    void Awake()
    {
        // Input Action Event Setting
        InputActionMap playerActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        
        moveAction = playerActionMap.FindAction("Move");
        lookAction = playerActionMap.FindAction("Look");
        jumpAction = playerActionMap.FindAction("Jump");
        inventoryAction = playerActionMap.FindAction("Inventory");
        interactAction = playerActionMap.FindAction("Interaction");
    }

    public void BindInput(InputActionType type, InputActionPhase phase, Action<InputAction.CallbackContext> callback)
    {
        switch (phase)
        {
            case InputActionPhase.Started:
                GetInputAction(type).started -= callback; // 중복 방지
                GetInputAction(type).started += callback;
                break;
            case InputActionPhase.Performed:
                GetInputAction(type).performed -= callback;
                GetInputAction(type).performed += callback;
                break;
            case InputActionPhase.Canceled:
                GetInputAction(type).canceled -= callback;
                GetInputAction(type).canceled += callback;
                break;
            default:
                Debug.LogError("Bind Input Action Phase is Null");
                break;
        }
    }
    
    InputAction GetInputAction(InputActionType type)
    {
        switch (type)
        {
            case InputActionType.Move:
                return moveAction;
            case InputActionType.Look:
                return lookAction;
            case InputActionType.Jump:
                return jumpAction;
            case InputActionType.Inventory:
                return inventoryAction;
            case InputActionType.Interact:
                return interactAction;
            default:
                Debug.LogError("Bind Input Action Type(enum) is Null");
                return null;
        }
    }
}
