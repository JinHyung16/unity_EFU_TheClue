using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using System;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameSetUpData gameSetUpData; //game setup data

    [Header("Player Transform")]
    [SerializeField] private Transform playerTransform; //움직이는 player의 Transform을 담을 변수

    [Header("Player Camera")]
    [SerializeField] private GameObject playerCamera = null; //scene에서 찾은 main camera를 담을 변수

    private PlayerInputController playerInputController;
    
    private void OnEnable()
    {
        playerInputController = GetComponent<PlayerInputController>();
        playerInputController.cameraView = playerCamera.transform;
    }

    public void PlayerSetUp()
    {
        playerTransform.position = gameSetUpData.characterTransform;

        playerCamera.transform.position = gameSetUpData.cameraPosition;
        playerCamera.transform.rotation = Quaternion.Euler(gameSetUpData.cameraRotation);
    }
}
