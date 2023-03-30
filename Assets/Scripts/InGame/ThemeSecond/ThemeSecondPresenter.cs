using DG.Tweening;
using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondPresenter : PresenterSingleton<ThemeSecondPresenter>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private ThemeSecondViewer themeSecondViewer;

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;

    private string themeName = "ThemeSecond";
    protected override void OnAwake()
    {
        switchSpotLight.color = Color.white;
        switchSpotLight.enabled = true;
    }

    private void Start()
    {
        themeName = "ThemeSecond";
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.themeCamera = this.mainCamera;
        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        TimerManager.GetInstance.ThemeTime = 900.0f;
    }

    public void SwitchOnOff()
    {
        if (switchSpotLight.enabled)
        {
            switchSpotLight.enabled = false;
        }
        else
        {
            switchSpotLight.enabled = true;
            if (TimerManager.GetInstance.CurMinTime <= 10)
            {
                switchSpotLight.color = Color.blue;
            }
            else
            {
                switchSpotLight.color = Color.red;
            }
        }
    }
}
