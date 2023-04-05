using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristWatch : MonoBehaviour
{
    [SerializeField] private Transform watchInShowcaseTransform;

    public void PutDownWristWatch()
    {
        this.gameObject.transform.position = watchInShowcaseTransform.position;
    }
}
