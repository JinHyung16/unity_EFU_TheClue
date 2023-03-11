using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondPresenter : PresenterSingleton<ThemeSecondPresenter>
{
    [SerializeField] private Camera mainCamera;

    private string themeName = "ThemeSecond";
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
