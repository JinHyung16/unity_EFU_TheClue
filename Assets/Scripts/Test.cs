using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private KeyCode sceneLoadKey;

    private void Start()
    {
        sceneLoadKey = KeyCode.Space;
    }

    private void Update()
    {
        SceneLoadTest();
    }

    private void SceneLoadTest()
    {
        if (Input.GetKeyDown(sceneLoadKey))
        {
            SceneController.GetInstace.LoadSceneAsync("ThemeOne").Forget();
        }
    }
}
