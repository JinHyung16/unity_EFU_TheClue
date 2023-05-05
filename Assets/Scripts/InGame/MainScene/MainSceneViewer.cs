using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainSceneViewer : MonoBehaviour
{
    [Header("mainScene에서 열고 닫을 패널들")]
    [SerializeField] private List<GameObject> mainScenePanelList = new List<GameObject>();

    [Header("ThemeSelectPanel 하위 테마 선택 버튼들")]
    [SerializeField] private List<Button> themeSelectBtnList = new List<Button>(); //테마 순서대로 저장

    private void Awake()
    {
        foreach (var panel in mainScenePanelList)
        {
            UIManager.GetInstance.AddPanelInDictionary(panel.name, panel);
        }

        for (int i = 0; i < themeSelectBtnList.Count; i++)
        {
            themeSelectBtnList[i].interactable = false;
        }
    }

    private void Start()
    {
        for (int i = 0; i < MainScenePresenter.GetInstance.ThemeSelectIndex; i++)
        {
            themeSelectBtnList[i].interactable = true;
        }
    }
    private void OnDisable()
    {
        if (UIManager.GetInstance != null)
        {
            UIManager.GetInstance.ClearAllPanel();
        }
    }

    #region Button Functions
    /// <summary>
    /// 게임 시작 버튼을 누르면 호출하여 테마 선택 창을 보여준다.
    /// 호출시 어떤 창을 열건지 이름을 전달해준다.
    /// </summary>
    public void ThemeSelectButton()
    {
        UIManager.GetInstance.ShowPanel("ThemeSelect Panel");
    }

    public void SettingButton()
    {
        UIManager.GetInstance.ShowPanel("Setting Panel");
    }

    /// <summary>
    /// 패널에 붙어있는 닫기 버튼의 연결되는 함수다.
    /// 해당 씬의 모든 닫기 버튼은 이 함수와 연결되어야한다.
    /// </summary>
    public void ClosePanelButton()
    {
        UIManager.GetInstance.HidePanel();
    }

    /// <summary>
    /// ThemeSelect Panel에서 열려져있는 테마를 선택해 플레이한다.
    /// </summary>
    public void ThemeSelect()
    {
        string themeName = EventSystem.current.currentSelectedGameObject.GetComponent<ThemeSelectData>().ThemeName;
        MainScenePresenter.GetInstance.ThemeSelectedAndLoadScene(themeName);
    }
    #endregion
}
