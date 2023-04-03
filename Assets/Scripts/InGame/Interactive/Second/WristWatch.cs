using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristWatch : MonoBehaviour
{
    [SerializeField] private Transform watchInShowcaseTransform;
    private void Start()
    {
        Debug.Log(watchInShowcaseTransform.position.x + " " + watchInShowcaseTransform.position.y + " " + watchInShowcaseTransform.position.z);
    }
    public void PutDownWristWatch()
    {
        this.gameObject.transform.position = watchInShowcaseTransform.position;
    }
}
