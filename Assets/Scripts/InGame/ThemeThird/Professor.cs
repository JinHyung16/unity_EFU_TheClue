using HughFSM;
using HughPathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation(int index)
    {
        switch (index)
        {
            case 0:
                animator.SetBool("IsRun", false);
                animator.SetBool("IsSurprised", false);
                animator.SetBool("IsLock", false);
                break;
            case 1:
                animator.SetBool("IsRun", true);
                break;
            case 2:
                animator.SetBool("IsSurprised", true);
                break;
            case 3:
                animator.SetBool("IsLock", true);
                break;
        }
    }
}
