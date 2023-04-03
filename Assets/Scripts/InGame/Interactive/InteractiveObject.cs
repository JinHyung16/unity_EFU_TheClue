using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public abstract class InteractiveObject : MonoBehaviour
{
    protected abstract void OnTriggerEnter(Collider other);

    protected abstract void OnTriggerExit(Collider other);
    protected abstract void Interacitve();

    protected abstract void NotInteractvie();

    public abstract InteractiveType GetInteractiveType();
}
