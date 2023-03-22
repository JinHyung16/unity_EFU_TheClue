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
    [SerializeField] private Transform playerCameraTrans; //player mainCamera 그 자체

    private void Start()
    {
        if (gameSetUpData == null)
        {
            gameSetUpData = Resources.Load("Data/GameSetUpData") as GameSetUpData;
        }
    }

    private void OnEnable()
    {
        playerTransform.position = gameSetUpData.characterTransform;
        playerCameraTrans.position = new Vector3(0, 0, gameSetUpData.cameraPosZ);
        cameraViewTrans.position = new Vector3(0, gameSetUpData.cameraViewPosY, 0);
    }

    public Camera PlayerCamera()
    {
        return this.playerCamera;
    }
}
