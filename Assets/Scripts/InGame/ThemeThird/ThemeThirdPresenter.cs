using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeThirdPresenter : PresenterSingleton<ThemeThirdPresenter>
{
    [SerializeField] private Camera mainCamera;

    private string themeName = "ThemeThird";
    protected override void OnAwake()
    {
        //cameraMain.enabled = false;
        mainCamera.cullingMask = 5;
    }

    private void Start()
    {
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
    }
}
