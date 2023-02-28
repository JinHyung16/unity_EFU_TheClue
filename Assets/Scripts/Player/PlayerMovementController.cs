using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private GameSetUpData gameSetUpData;

    private Rigidbody playerRigidbody;

    private Vector3 moveDirection;
    private float moveSpeed;
    private float jumpSpeed;

    private void Start()
    {
        playerRigidbody = GetComponentInChildren<Rigidbody>();

        //bind player movement data
        this.moveSpeed = gameSetUpData.moveSpeed;
        this.jumpSpeed = gameSetUpData.jumpPower;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void MoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void Movement()
    {
        playerRigidbody.velocity = moveDirection * moveSpeed;
    }
}
