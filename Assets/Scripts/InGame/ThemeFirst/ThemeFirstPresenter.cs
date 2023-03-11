using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ThemeFirstPresenter : PresenterSingleton<ThemeFirstPresenter>
{
    [SerializeField] private Camera mainCamera;

    //Game끝낼 때 GameProgress에 넘겨줄 현재 테마 이름 정보로 각 ThemePresenter들이 이 정보를 갖고 있는다.
    private string themeName = "ThemeFirst";

    protected override void OnAwake()
    {
        //mainCamera.enabled = false;
        mainCamera.cullingMask = 5;
    }

    private void Start()
    {
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
    }
}
