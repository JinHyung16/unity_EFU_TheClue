using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    public Transform cameraViewTrans { private get; set; }

    private PlayerMovementController playerMovement;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        InputMove();
        InputMouse();
    }

    private void InputMove()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        //animator.SetBool("IsMove", isMove);

        if (isMove)
        {
            //캐릭터 움직일 때 카메라가 바라보는 방향을 바라보게 수정
            Vector3 lookForward = new Vector3(cameraViewTrans.forward.x, 0.0f, cameraViewTrans.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraViewTrans.right.x, 0.0f, cameraViewTrans.right.z).normalized;
            Vector3 moveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);

            playerTransform.forward = lookForward; //player가 바라보는 방향과 카메라가 바라보는 방향 동일하게 설정
            playerMovement.MoveDirection(moveDir);
        }
    }

    private void InputMouse()
    {
        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 cameraAngle = cameraViewTrans.rotation.eulerAngles;

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
        cameraViewTrans.rotation = Quaternion.Euler(rotateX, cameraAngle.y + mousePos.x, cameraAngle.z);
    }
}
