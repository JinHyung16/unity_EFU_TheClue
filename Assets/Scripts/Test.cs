using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private KeyCode sceneLoadThemeFirstKey;
    [SerializeField] private KeyCode sceneLoadThemeSecondey;
    [SerializeField] private KeyCode sceneLoadThemeThirdKey;

    private void Start()
    {
        sceneLoadThemeFirstKey = KeyCode.F1;
        sceneLoadThemeSecondey = KeyCode.F2;
        sceneLoadThemeThirdKey = KeyCode.F3;
    }

    private void Update()
    {
        SceneLoadTest();
    }

    private void SceneLoadTest()
    {
        if (Input.GetKeyDown(sceneLoadThemeFirstKey))
        {
            SceneController.GetInstance.LoadScene("ThemeFirst");
        }
        else if (Input.GetKeyDown(sceneLoadThemeSecondey))
        {
            SceneController.GetInstance.LoadScene("ThemeSecond");
        }
        else if (Input.GetKeyDown(sceneLoadThemeThirdKey))
        {
            SceneController.GetInstance.LoadScene("ThemeThird");
        }
    }
}
