using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    //private Vector3 screenCenter;
    private float moveToCamRotateSpeed;
    private List<KeyCode> invenSelectKeyList;

    private GameObject hitObj;
    private int inputNum = 0;
    private void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        playerAnimator = GetComponentInChildren<Animator>();

        moveToCamRotateSpeed = gameSetUpData.moveToCamRotateSpeed;

        invenSelectKeyList = new List<KeyCode>
        {
            gameSetUpData.firstInvenSelectKey,
            gameSetUpData.secondInvenSelectKey,
            gameSetUpData.thirdInvenSelectKey
        };

        //Mouse Cursor 설정
        //screenCenter = new Vector3(playerCamera.pixelWidth / 2, playerCamera.pixelHeight / 2);

#if !UNITY_EDITOR
    Cursor.lockState = CursorLockMode.Confined;
#endif
    }

    private void Update()
    {
        if (!GameManager.GetInstance.IsDialogueStart || !GameManager.GetInstance.IsGameClear)
        {
            //InputOpenOption();
            InputSelectInventory();
            InputCancelInteractiveKey();
            InputMouseRay();

            //Option 창이 열림 또는 UI가 열려있는 상태면 나머지 아래 동작들은 작동시키지 못하게 한다.
            if (!GameManager.GetInstance.IsInputStop && !GameManager.GetInstance.IsUIOpen)
            {
                InputMovementControl();
                InputMouseViewControl();
                InputInteractiveKey();
            }
        }
    }

    /// <summary>
    /// 상하좌우 움직임 입력
    /// </summary>
    private void InputMovementControl()
    {
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            AudioManager.GetInstance.PlaySFX(AudioManager.SFX.PlayerMove);
        }
        Vector2 moveInput = new Vector2(horizontal, vertical);
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
        //cameraMain.x rotate하면 위 아래로 회전하고, cameraMain.y rotate하면 좌우로 회전한다

        //cameraView.rotation = Quaternion.Euler(rotateX, cameraAngle.y + mousePos.x, cameraAngle.z);
        Quaternion rotateCam = Quaternion.Euler(rotateX, cameraAngle.y + mousePos.x, cameraAngle.z);
        cameraView.rotation = Quaternion.Slerp(cameraView.rotation, rotateCam, moveToCamRotateSpeed);
        
    }

    
    private void InputMouseRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.GetInstance.CameraInteractive != null)
            {
                Ray ray = GameManager.GetInstance.CameraInteractive.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject != null)
                    {
                        if (hit.collider.gameObject.CompareTag("WristWatch"))
                        {
                            if (hitObj != null)
                            {
                                hitObj.GetComponent<WristWatch>().PutDownWristWatch();
                                hitObj = null;
                            }
                            else
                            {
                                hitObj = hit.collider.gameObject;
                                hit.collider.gameObject.transform.position = GameManager.GetInstance.CameraInteractive.transform.position + new Vector3(0, -0.5f, 0);
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (GameManager.GetInstance.CameraInteractive != null && ThemeSecondPresenter.GetInstance != null)
            {
                if (ThemeSecondPresenter.GetInstance.InteractiveTypeNum == 1)
                {
                    GameObject obj = InventoryManager.GetInstance.GetInvenObject();
                    if (obj != null)
                    {
                        obj.transform.Rotate(0.0f, -Input.GetAxis("Mouse X") * gameSetUpData.moseDragSpeed, 0.0f, Space.World);
                        obj.transform.Rotate(-Input.GetAxis("Mouse Y") * gameSetUpData.moseDragSpeed, 0.0f, 0.0f);
                    }
                }
            }
        }
    }
    private void InputOpenOption()
    {
        if (Input.GetKeyDown(gameSetUpData.escKey))
        {
            GameManager.GetInstance.OptionCanvasOpen(true);
        }
    }

    private void InputSelectInventory()
    {
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

        if (Input.GetKeyDown(gameSetUpData.throwOutInvenKey))
        {
            if (InventoryManager.GetInstance != null)
            {
                if (GameManager.GetInstance.IsUIOpen)
                {
                    if (ThemeSecondPresenter.GetInstance != null && ThemeSecondPresenter.GetInstance.InteractiveTypeNum == 1)
                    {
                        ThemeSecondPresenter.GetInstance.PutInTheDoor(InventoryManager.GetInstance.GetInvenObject());
                    }
                }
                else
                {
                    InventoryManager.GetInstance.ThrowOutInventoryObject(inputNum);
                }
            }
        }
    }

    private void InputInteractiveKey()
    {
        if (Input.GetKeyDown(gameSetUpData.interactiveKey))
        {
            if (InteractiveManager.GetInstance.IsInteractive)
            {
                InteractiveManager.GetInstance.Interactive();
            }
        }
    }

    private void InputCancelInteractiveKey()
    {
        if (Input.GetKeyDown(gameSetUpData.escKey))
        {
            if (ThemeFirstPresenter.GetInstance != null && ThemeFirstPresenter.GetInstance.InteractiveUIOpen)
            {
                ThemeFirstPresenter.GetInstance.CloseCanvase();
            }
            else if (ThemeSecondPresenter.GetInstance != null)
            {
                switch (ThemeSecondPresenter.GetInstance.InteractiveTypeNum)
                {
                    case 1:
                        ThemeSecondPresenter.GetInstance.DoorKeyHoleInteractive(false);
                        break;
                    case 2:
                        ThemeSecondPresenter.GetInstance.ShowCaseInteractive(false);
                        break;
                    case 3:
                        ThemeSecondPresenter.GetInstance.NoteOpen(null, false);
                        break;
                    case 4:
                        ThemeSecondPresenter.GetInstance.CloseCanvas();
                        break;
                    default:
                        GameManager.GetInstance.OptionCanvasOpen(true);
                        break;
                }
            }
            else
            {
                GameManager.GetInstance.OptionCanvasOpen(true);
            }
        }
    }
}
