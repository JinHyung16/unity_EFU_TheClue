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

    private Transform themeCameraTransform;

    private void Start()
    {
        playerTransform.position = gameSetUpData.characterTransform;
        playerCameraTrans.position = playerTransform.position + new Vector3(0, 0, gameSetUpData.cameraPosZ);
        cameraViewTrans.position = playerTransform.position + new Vector3(0, gameSetUpData.cameraViewPosY, 0);

        if (Time.timeScale == 0)
        {
            Debug.Log("PlayerManager에서 Time.timeScale 측정: " + Time.timeScale);
            Time.timeScale = 1;
        }
    }

    private void OnDisable()
    {
        themeCameraTransform.SetParent(null);
        themeCameraTransform = null;
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
