using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ThemeThirdPresenter.GetInstance.HeadStoneNarrative();
    }
}
