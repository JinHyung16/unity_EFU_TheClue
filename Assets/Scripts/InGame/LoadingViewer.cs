using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoadingViewer : MonoBehaviour
{
    private void Start()
    {
        SceneController.GetInstance.LoadThemeScene().Forget();
    }
}
