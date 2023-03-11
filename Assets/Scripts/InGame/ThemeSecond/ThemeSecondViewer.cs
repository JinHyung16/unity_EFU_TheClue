using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondViewer : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAll();
    }
}
