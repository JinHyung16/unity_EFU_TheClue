using DG.Tweening;
using HughPathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 moveDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        PathManager.GetInstance.SetTargetPosition(this.transform);
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        rigid.velocity = moveDir * 7.0f;
    }
}
