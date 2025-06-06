using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Camera")]
    [SerializeField] private Camera playerCamera;
    
    [Header("GameSetUpData")]
    [SerializeField] private GameSetUpData gameSetUpData;

    [Header("Player Transform")]
    [SerializeField] private Transform playerTransform; //움직이는 player의 Transform을 담을 변수

    [Header("Player Camera ViewTransform")]
    [SerializeField] private Transform cameraViewTrans; //player camera의 부모 오브젝트인 CameraView

    [Header("Player Camera Transform")]
    [SerializeField] private Transform playerCameraTrans; //player cameraMain 그 자체

    [Header("Player Camera Sub Transform")]
    [SerializeField] private Transform playerCameraSubTrans; //player cameraMain 그 자체

    private Transform themeCameraTransform;

    private void Start()
    {
        if (ThemeFirstPresenter.GetInstance != null)
        {
            playerTransform.position = gameSetUpData.themeFirstSpawnPos;
        }
        else if (ThemeSecondPresenter.GetInstance != null)
        {
            playerTransform.position = gameSetUpData.themeSecondSpawnPos;
        }
        else if (ThemeThirdPresenter.GetInstance != null)
        {
            playerTransform.position = gameSetUpData.themeThirdSpawnPos;
        }
        //playerTransform.position = gameSetUpData.themeFirstSpawnPos;
        playerCameraTrans.position = playerTransform.position + new Vector3(0, 0, gameSetUpData.cameraPosZ);
        playerCameraSubTrans.position = playerCameraTrans.position;
        cameraViewTrans.position = playerTransform.position + new Vector3(0, gameSetUpData.cameraViewPosY, 0);
    }

    private void OnDisable()
    {
        if (themeCameraTransform != null)
        {
            themeCameraTransform.SetParent(null);
            themeCameraTransform = null;
        }
    }

    public Camera PlayerCamera()
    {
        return this.playerCamera;
    }

    public void SetParentCamera(Transform trans)
    {
        themeCameraTransform = trans;
        trans.SetParent(cameraViewTrans);
        trans.position = playerTransform.position + new Vector3(0, 0, gameSetUpData.cameraPosZ);
        trans.rotation = playerCamera.transform.rotation;
    }

}
