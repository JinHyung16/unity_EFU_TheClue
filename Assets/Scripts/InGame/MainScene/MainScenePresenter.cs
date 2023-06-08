using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;

public class MainScenePresenter : PresenterSingleton<MainScenePresenter>
{
    [SerializeField] private MainSceneViewer mainSceneViewer;
    private int LoadThemeIndex = 0;

    private void Start()
    {
        if (GameManager.GetInstance.IsFirstGame)
        {
            LoadThemeIndex = DataManager.GetInstance.LoadData().themeClearIndex;
        }
        else
        {
            if (DataManager.GetInstance.SaveThemeIndex <= 0)
            {
                DataManager.GetInstance.SaveThemeIndex = 1;
                LoadThemeIndex = DataManager.GetInstance.SaveThemeIndex;
            }
            LoadThemeIndex = DataManager.GetInstance.SaveThemeIndex;
        }
        mainSceneViewer.ThemeSelectOpen(LoadThemeIndex);
    }

    public void ThemeSelectedAndLoadScene(string theme)
    {
        SceneController.GetInstance.LoadScene(theme);
    }

}
