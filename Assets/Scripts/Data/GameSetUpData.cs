using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameSetUpData", menuName = "ScriptableObjects/GameSetUpData", order = 1)]
public class GameSetUpData : ScriptableObject
{
    //Player Camera 위치 세팅
    [Header("Player Camera  Z position Setting Data")]
    public float cameraPosZ;

    //Player Camera를 갖고있는 Camera View의 세팅
    [Header("Camera View Y position Setting Data")]
    public float cameraViewPosY;

    //Character 하위에 있는 Player와 CameraView 위치 세팅
    [Header("CameraView Setting into Character Object")]
    public Vector3 characterTransform;

    //PlayerMovement의 사용되는 데이터
    [Header("Player Movement Data")]
    public float moveSpeed;
    public float jumpPower;
    public float cameraRotateSpeed;

    [Header("Player Input Keyboard")]
    public KeyCode optionKey = KeyCode.Escape;
    public KeyCode inventoryKey = KeyCode.E;
}
