using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerMovementController playerMovementController;

    [Header("Player Transform")]
    [SerializeField] private Transform playerTransform;

    public Transform cameraView { private get; set; }

    private void Start()
    {
        playerMovementController = GetComponentInChildren<PlayerMovementController>();
    }

    private void Update()
    {
        if (!GameManager.GetInstance.IsGamePause)
        {
            InputMovementControl();
            InputJumpControl();
            InputMouseViewControl();
        }
    }

    private void InputMovementControl()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        bool isMove = moveInput.magnitude != 0;
        //animator.SetBool("IsMove", isMove);
        if (isMove)
        {
            //Character -> Player & Camera (둘이 대등한) 구조이면 사용하기
            /*
            //캐릭터 움직일 때 카메라가 바라보는 방향을 바라보게 수정
            Vector3 lookForward = new Vector3(cameraView.forward.x, 0.0f, cameraView.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraView.right.x, 0.0f, cameraView.right.z).normalized;
            Vector3 moveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);

            playerTransform.forward = lookForward; //player가 바라보는 방향과 카메라가 바라보는 방향 동일하게 설정
            */
            playerMovementController.MoveDirection(moveInput);
        }
        
    }

    private void InputJumpControl()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerMovementController.JumpDirection(Vector3.up);
        }
    }


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
        //camera.x rotate하면 위 아래로 회전하고, camera.y rotate하면 좌우로 회전한다
        cameraView.rotation = Quaternion.Euler(rotateX, cameraAngle.y + mousePos.x, cameraAngle.z);
    }
}
