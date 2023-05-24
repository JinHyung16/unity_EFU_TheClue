using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThemeThirdPresenter.GetInstance.HeadStoneNarrative();
        }
    }

    /*  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ThemeThirdPresenter.GetInstance.HeadStoneNarrative();
        }
    }
    */
}
