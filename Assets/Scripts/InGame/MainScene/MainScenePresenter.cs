using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.UI;

public class MainScenePresenter : PresenterSingleton<MainScenePresenter>
{
    [SerializeField] private MainSceneViewer mainSceneViewer;

    private int savedThemeCnt = 1; //기본적으로 테마1은 열려있어야 하므로
    
    private void Start()
    {
        var gameProgressData = DataManager.GetInstance.LoadData();
        if (gameProgressData != null)
        {
            savedThemeCnt = gameProgressData.themeClearIndex;
            mainSceneViewer.EnableThemeSelectBtns(savedThemeCnt);
        }
        else
        {
            mainSceneViewer.EnableThemeSelectBtns(savedThemeCnt);
        }
    }

    public void ThemeSelectedAndLoadScene(string theme)
    {
        SceneController.GetInstance.LoadScene(theme);
    }
}
