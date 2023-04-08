using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public class PlayerAnimationController : MonoBehaviour
{
    #region Static
    public static PlayerAnimationController GetInstance;

    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    private Animator animator;

    public enum AnimationLayerName
    {
        Died,
        DoorLock,
        OpenDoor,
        PickUp,
        TileInteractive,
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayerAnimationControl(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.P_PickUp:
                animator.SetTrigger("onPickUp");
                break;
            case AnimationType.P_EnterCode:
                animator.SetTrigger("onEnterCode");
                break;
            case AnimationType.P_OpenDoor:
                animator.SetTrigger("onOpenDoor");
                break;
            case AnimationType.P_Died:
                animator.SetTrigger("onDied");
                break;
            default:
                break;
        }
    }

    private void AnimationLayerActivate(AnimationLayerName name)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (animator.GetLayerName(i) == "IdleAndWalkLayer")
            {
                continue;
            }
            else
            {
                animator.SetLayerWeight(1, 0);
            }
        }

        animator.SetLayerWeight((int)name, 1);
    }
}
