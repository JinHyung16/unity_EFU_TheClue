using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputController : MonoBehaviour
{
    private PlayerMovementController playerMovementController;
    private Animator playerAnimator;

    [Header("Player Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("GameSetUpData")]
    [SerializeField] private GameSetUpData gameSetUpData;

    [Header("Player Transform")]
    [SerializeField] private Transform playerTransform;
    
    [Header("Player Camera Transform")]
    [SerializeField] private Transform cameraView;


    private float cameraRotateSpeed;
    private KeyCode optionKey;
    private KeyCode interactiveKey;

    private List<KeyCode> invenSelectKeyList;
    private KeyCode firstInvenSelect;
    private KeyCode secondInvenSelect;
    private KeyCode thirdInvenSelect;
    
    private void Start()
    {
        if (gameSetUpData == null)
        {
            gameSetUpData = Resources.Load("Data/GameSetUpData") as GameSetUpData;
        }

        playerMovementController = GetComponent<PlayerMovementController>();
        playerAnimator = GetComponentInChildren<Animator>();

        Cursor.visible = true;

        cameraRotateSpeed = gameSetUpData.cameraRotateSpeed;
        this.optionKey = gameSetUpData.optionKey;
        this.interactiveKey = gameSetUpData.interactiveKey;

        invenSelectKeyList = new List<KeyCode>
        {
            gameSetUpData.firstInvenSelectKey,
            gameSetUpData.secondInvenSelectKey,
            gameSetUpData.thirdInvenSelectKey
        };
    }

    private void Update()
    {
        InputOpenOption();
        InputSelectInventory();
        //Option 창이 열림 또는 UI가 열려있는 상태면 나머지 아래 동작들은 작동시키지 못하게 한다.
        if (!GameManager.GetInstance.IsInputStop && !GameManager.GetInstance.IsUIOpen)
        {
            InputMovementControl();
            InputJumpControl();
            InputMouseViewControl();
            InputInteractive();
        }
    }

    /// <summary>
    /// 상하좌우 움직임 입력
    /// </summary>
    private void InputMovementControl()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveInput = new Vector2(horizontal * cameraRotateSpeed, vertical * cameraRotateSpeed);

        playerAnimator.SetFloat("Horizontal", horizontal);
        playerAnimator.SetFloat("Vertical", vertical);
        //bool isMove = moveInput.magnitude != 0.0f;
        //animator.SetBool("IsMove", isMove);

        //캐릭터 움직일 때 카메라가 바라보는 방향을 바라보게 수정
        Vector3 lookForward = new Vector3(cameraView.forward.x, 0.0f, cameraView.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraView.right.x, 0.0f, cameraView.right.z).normalized;
        Vector3 moveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);

        playerTransform.forward = lookForward; //player가 바라보는 방향과 카메라가 바라보는 방향 동일하게 설정
        playerMovementController.MoveDirection(moveDir); //moveDir방향으로 움직이고, 만약 1인칭 시점으로 바뀌면 moveInput 넣는다.

    }

    /// <summary>
    /// player 점프 입력
    /// </summary>
    private void InputJumpControl()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerMovementController.JumpDirection(Vector3.up);
        }
    }

    /// <summary>
    /// 마우스 좌우 움직이면 카메라 회전
    /// </summary>
    private void InputMouseViewControl()
    {
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 cameraAngle = cameraView.rotation.eulerAngles;
        //카메라가 수직으로 너무 회전하면 뒤집히는 문제 해결
        float rotateX = cameraAngle.x - mousePos.y;
        if (rotateX < 180.0f)
        {
            //-1.0f가 최소여야 카메라가 수평면 아래로 내려간다.
            rotateX = Mathf.Clamp(rotateX, -1.0f, 70.0f);
        }
        else
        {
            //25도 각도 제한하기 위해 360.0f - 25.0f한 값으로 넣고
            //361.0f가 최대여야 카메라가 수평면 위로 잘 올라간다.
            //-1.0f가 최저여야 카메라가 수평면 아래로 내려간다.
            rotateX = Mathf.Clamp(rotateX, 335.0f, 361.0f);
        }
        //마우스 좌우 움직임으로 카메라 좌우 움직임 제어, 마우스 상하 움직임으로 카메라 상하 움직임 제어
        //mainCamera.x rotate하면 위 아래로 회전하고, mainCamera.y rotate하면 좌우로 회전한다
        cameraView.rotation = Quaternion.Euler(rotateX, cameraAngle.y + mousePos.x, cameraAngle.z);
    }

    /// <summary>
    /// 마우스 좌클릭
    /// </summary>
    private void InputMouseLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }
    }

    private void InputOpenOption()
    {
        if (Input.GetKeyDown(optionKey))
        {
            GameManager.GetInstance.OptionCanvasOpen(true);
        }
    }

    private void InputSelectInventory()
    {
        int inputNum = 0;
        if (Input.anyKeyDown)
        {
            foreach (var key in invenSelectKeyList)
            {
                if (Input.GetKeyDown(key))
                {
                    inputNum = (int.Parse(key.ToString().Substring(key.ToString().Length - 1))) - 1;
                    if (InventoryManager.GetInstance != null)
                    {
                        InventoryManager.GetInstance.SelectInventory(inputNum);
                    }
                }
            }
        }
    }

    private void InputInteractive()
    {
        if (Input.GetKeyDown(interactiveKey))
        {
            if (GameManager.GetInstance.IsInteractive)
            {
                InteractiveManager.GetInstance.Interactive();
            }
        }
    }

}
