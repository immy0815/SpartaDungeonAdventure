using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    
    public GameObject curInteractGameObject;
    private IInteractable curInteractable;
    
    // public TextMeshProUGUI promptText;
    private Camera mainCamera;

    void Reset()
    {
        checkRate = 0.05f;
        maxCheckDistance = 5;
        layerMask = LayerMask.GetMask("Interactable");
    }
    
    void Start()
    {
        mainCamera = Camera.main;
        
        PlayerInputSystem playerInputSystem = CharacterManager.Instance.Player.PlayerInputSystem;
        playerInputSystem.BindInput(InputActionType.Interact, InputActionPhase.Started, OnInteractInput);
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    // 프롬프트 출력
                    UIManager.Instance.OpenPrompt(curInteractable.GetInteractPrompt());
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                UIManager.Instance.ClosePrompt();
            }
        }
    }
    
    void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            CharacterManager.Instance.Player.OnAddItem?.Invoke(curInteractable.GetItemData());
            curInteractGameObject = null;
            curInteractable = null;
            
            UIManager.Instance.ClosePrompt();
        }
    }
}
