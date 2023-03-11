using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScene : MonoBehaviour
{
    [Header("Opening Canvas")]
    [SerializeField] private Canvas openingCanvas;

    [Header("Start Opening Canvas")]
    [SerializeField] private Canvas startOpeningCanvas;

    [Header("Skip Opening Button")]
    [SerializeField] private Button skipBtn;

    [Header("Start Opening Button")]
    [SerializeField] private Button startBtn;

    private void Start()
    {
        openingCanvas.enabled = false;
        startOpeningCanvas.enabled = true;

        skipBtn.onClick.AddListener(SkipScenairo);
        startBtn.onClick.AddListener(StartOpening);
    }

    private void SkipScenairo()
    {
        SceneController.GetInstance.LoadScene("Main");
    }

    private void StartOpening()
    {
        openingCanvas.enabled = true;
        startOpeningCanvas.enabled = false;
    }
}
