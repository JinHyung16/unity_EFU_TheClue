using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameSetUpData", menuName = "ScriptableObjects/GameSetUpData", order = 1)]
public class GameSetUpData : ScriptableObject
{
    //Main Camera 위치 세팅
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    //Character 하위에 있는 Player와 CameraView 위치 세팅
    public Vector3 characterTransform;

    //PlayerMovement의 사용되는 데이터
    public float moveSpeed;
    public float jumpPower;
}
