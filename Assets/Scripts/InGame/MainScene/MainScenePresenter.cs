using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;

public class MainScenePresenter : PresenterSingleton<MainScenePresenter>
{
    [SerializeField] private MainSceneViewer mainSceneViewer;
    private int LoadThemeIndex = 1;

    private void Start()
    {
        if (GameManager.GetInstance.IsFirstGame)
        {
            LoadThemeIndex = DataManager.GetInstance.LoadData().themeClearIndex;
            GameManager.GetInstance.IsFirstGame = false;
            Debug.Log("1");
        }
        else
        {
            if (DataManager.GetInstance.SaveThemeIndex <= 0)
            {
                DataManager.GetInstance.SaveThemeIndex = 1;
                LoadThemeIndex = DataManager.GetInstance.SaveThemeIndex;
            }
            LoadThemeIndex = DataManager.GetInstance.SaveThemeIndex;
            Debug.Log("2");
        }
        mainSceneViewer.ThemeSelectOpen(LoadThemeIndex);
    }

    public void ThemeSelectedAndLoadScene(string theme)
    {
        SceneController.GetInstance.LoadScene(theme);
    }

}
