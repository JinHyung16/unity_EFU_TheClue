using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public abstract class InteractiveObject : MonoBehaviour
{
    protected InteractiveType myInteractiveType;
    public abstract void InteracitveOrNot(bool interactive);
    public abstract InteractiveType GetInteractiveType();
}
