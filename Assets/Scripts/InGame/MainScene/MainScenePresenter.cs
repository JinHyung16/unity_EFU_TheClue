using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.UI;

public class MainScenePresenter : PresenterSingleton<MainScenePresenter>
{
    [SerializeField] private MainSceneViewer mainSceneViewer;
    private string themeName = "Main";
    public int ThemeSelectIndex { get; private set; } = 1;
    private void Start()
    {
        SceneController.GetInstance.CurSceneName = themeName;

        var gameProgressData = DataManager.GetInstance.LoadData();
        if (gameProgressData != null)
        {
            ThemeSelectIndex = gameProgressData.themeClearIndex;
        }
    }

    public void ThemeSelectedAndLoadScene(string theme)
    {
        SceneController.GetInstance.LoadScene(theme);
    }
}
