using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThemeThirdViewer : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAll();
    }
}
