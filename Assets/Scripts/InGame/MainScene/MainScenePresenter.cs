using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.UI;

public class MainScenePresenter : PresenterSingleton<MainScenePresenter>
{
    [SerializeField] private MainSceneViewer mainSceneViewer;

    private int clearThemeCnt = 1;
    protected override void OnAwake()
    {
        DataManager.GetInstance.LoadData();

        if (DataManager.GetInstance.IsDataLoad)
        {
            foreach (var data in DataManager.GetInstance.GameProgressDataArray)
            {
                if (data != null)
                {
                    clearThemeCnt++;
                }
            }
        }
    }
    
    private void Start()
    {
        mainSceneViewer.EnableThemeSelectBtns(clearThemeCnt);
    }

    public void ThemeSelectedAndLoadScene(string theme)
    {
        SceneController.GetInstance.LoadScene(theme);
    }
}
