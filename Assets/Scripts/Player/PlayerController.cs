using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("[Movement]")]
    public float moveSpeed;
    public float jumpPower;
    public LayerMask groundLayer;
    
    Rigidbody rigid;
    Vector2 curMovementInput;
    private bool isGrounded;
    
    // ===============================
    [Header("[Look]")]
    public Transform cameraContainer;
    public float minLook;
    public float maxLook;
    public float lookSensitivity;
    public bool canLook = true;
    
    private float camCurXRot;
    private Vector2 mouseDelta;
    
    // ===============================
    [Header("[Animation]")]
    public Animator animator;
    
    // ===============================
    // public Action InventoryAction;
    
    // ===============================================================================

    private void Reset()
    {
        ReferenceSetup();
        
        moveSpeed = 5;
        jumpPower = 80;
        minLook = -85;
        maxLook = 85;
        lookSensitivity = 0.1f;
        canLook = true;
    }

    void Awake()
    {
        ReferenceSetup();
    }

    private void Start()
    {
        EventBinding();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();  // 한 번만 호출
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsMoving", new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude > 0.1f);
        
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
        mouseDelta = Vector2.zero; // mouse delta 값 초기화 =>  계속 회전 될 가능성 개선
    }
    
    // ===============================================================================
    
    void ReferenceSetup()
    {
        rigid = GetComponent<Rigidbody>();
        cameraContainer = transform.Find("CameraContainer")?.GetComponent<Transform>();
        
        groundLayer = ~0; // 모든 레이어
        groundLayer &= ~LayerMask.GetMask("Player"); // Player 제외
        
        animator =  GetComponentInChildren<Animator>();
    }
    
    void EventBinding()
    {
        PlayerInputSystem playerInputSystem = CharacterManager.Instance.Player.PlayerInputSystem;
        
        playerInputSystem.BindInput(InputActionType.Move, InputActionPhase.Performed, OnMove);
        playerInputSystem.BindInput(InputActionType.Move, InputActionPhase.Canceled, OnMove);
        playerInputSystem.BindInput(InputActionType.Look, InputActionPhase.Performed, OnLook);
        playerInputSystem.BindInput(InputActionType.Jump, InputActionPhase.Performed, OnJump);
        playerInputSystem.BindInput(InputActionType.Inventory, InputActionPhase.Started, OnInventory);
    }
    
    // ===============================================================================
    
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigid.velocity.y; //  y 초기화

        rigid.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // ===============================================================================
    
    #region Input Action 콜백 함수
    
    /*
        [ phase == 분기점 ]
        Started: 실행 시작 시 호출
        Performed: 실행 확정 (완전히 실행) 시 호출
        Canceled: 실행 종료 시 호출
        Disabled: 액션이 활성화되지 않음
        Waiting: 액션이 활성화되어있고 입력을 기다리는 상태
    */
    
    // Input Action 콜백 함수 : Move
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            animator.SetBool("IsMoving", true);
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    // Input Action 콜백 함수 : Look
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // Input Action 콜백 함수 : Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CheckGrounded())
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
    
    // Input Action 콜백 함수 : Inventory
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // InventoryAction?.Invoke();
            ToggleCursor();
        }
    }

    #endregion   

    // ===============================================================================
    
    bool CheckGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            // 땅 아래에서 쏠 수 있기 때문에 방지용으로 0.1f를 곱해 줌
            // Ray(현재 위치 + (상/하/좌/우 위치 조정) + (방지용으로 0.1f만큼 위로 설정), Ray 방향)
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                return true;
            }
        }
        
        return false;
    }
    
    // 커서 On/Off
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
