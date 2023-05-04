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
    [Header("Character Object Spawn Position")]
    public Vector3 themeFirstSpawnPos;
    public Vector3 themeSecondSpawnPos;
    public Vector3 themeThirdSpawnPos;

    //PlayerMovement의 사용되는 데이터
    [Header("Player Movement And Input Data")]
    public float moveSpeed;
    public float cameraRotateSpeed;
    public float mouseDragSpeed;

    [Header("Player Input Keyboard")]
    public KeyCode optionKey = KeyCode.Escape;
    public KeyCode interactiveKey = KeyCode.E;
    public KeyCode notInteractiveKey = KeyCode.F;
    public KeyCode throwOutInvenKey = KeyCode.G;

    [Header("Input To Select Inventory")]
    public KeyCode firstInvenSelectKey = KeyCode.Alpha1;
    public KeyCode secondInvenSelectKey = KeyCode.Alpha2;
    public KeyCode thirdInvenSelectKey = KeyCode.Alpha3;
}
