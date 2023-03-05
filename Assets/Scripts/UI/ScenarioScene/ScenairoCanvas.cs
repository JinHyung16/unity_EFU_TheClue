using Cysharp.Threading.Tasks;
using HughCanvas;
using HughUIType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenairoCanvas : CanvasManager
{
    [SerializeField] private Button skipBtn;

    private void Start()
    {
        SceneController.GetInstance.SetCurScene();
        UIManager.GetInstance.AddCanvasInDictionary(CanvasType.FixedCanvas, this);
        if (DataManager.GetInstance.GetUserLoginRecord())
        {
            skipBtn.enabled = true;
            skipBtn.onClick.AddListener(SkipScenairo);
        }
        else
        {
            skipBtn.enabled = false;
        }
    }
    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAllCanvas();
    }

    private void SkipScenairo()
    {
        SceneController.GetInstance.LoadScene("ThemeFirst");
    }
}
