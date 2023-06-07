using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainScenePresenter : PresenterSingleton<MainScenePresenter>
{
    [SerializeField] private MainSceneViewer mainSceneViewer;

    private void Start()
    {
        Debug.Log("MainScene");
        if (DataManager.GetInstance.SaveThemeIndex <= 0)
        {
            DataManager.GetInstance.SaveThemeIndex = 1;
        }
        mainSceneViewer.ThemeSelectOpen(DataManager.GetInstance.SaveThemeIndex);
    }

    public void ThemeSelectedAndLoadScene(string theme)
    {
        SceneController.GetInstance.LoadScene(theme);
    }

}
