using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameSetUpData gameSetUpData; //game setup data

    [SerializeField] private Transform cameraViewTransform; //카메라가 해당 오브젝트 자식으로 있어야한다.
    [SerializeField] private Transform playerTransform; //움직이는 player의 Transform을 담을 변수

    private GameObject mainCamera; //scene에서 찾은 main camera를 담을 변수

    private PlayerInputController playerInputController;
    private void Awake()
    {
        //Character 오브젝트 하위는 Player와 CameraView로 나뉘므로 이 둘의 위치를 동기화 시켜준다.
        cameraViewTransform.position = gameSetUpData.characterTransform;
        playerTransform.position = gameSetUpData.characterTransform;

        //씬의 있는 카메라를 찾아서 하위에 두고 같이 움직인다.
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.parent = this.cameraViewTransform;
        mainCamera.transform.position = gameSetUpData.cameraPosition;
        mainCamera.transform.rotation = Quaternion.Euler(gameSetUpData.cameraRotation);

        playerInputController = GetComponent<PlayerInputController>();
        playerInputController.cameraViewTrans = mainCamera.transform;
    }


    private void Update()
    {
        //카메라가 player를 계속 쫓아간다. 이때, cameraView가 mainCamera의 상위이므로 이걸 이동시킨다
        cameraViewTransform.transform.position = playerTransform.position;

        /*
        new Vector3(playerTransform.position.x, 
            playerTransform.position.y + gameSetUpData.cameraPosition.y, 
            playerTransform.position.z + gameSetUpData.cameraPosition.z
            );
        */
    }
}
