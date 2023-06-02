using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingGageText;

    private void Start()
    {
        SceneController.GetInstance.LoadThemeScene().Forget();
    }

    private void Update()
    {
        loadingGageText.text = "테마를 불러오는 중.." + SceneController.GetInstance.GetLoadRatio;
    }
}
