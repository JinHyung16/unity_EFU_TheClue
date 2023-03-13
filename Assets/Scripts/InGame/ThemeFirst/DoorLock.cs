using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField] private Transform doorLockObjTransform;

    private Vector3 offset = Vector3.zero;
    private void Start()
    {
        offset = new Vector3(0, 0.3f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThemeFirstPresenter.GetInstance.VisibleInteractiveUI(doorLockObjTransform, offset);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThemeFirstPresenter.GetInstance.InvisibleInteractiveUI();
        }
    }
}
