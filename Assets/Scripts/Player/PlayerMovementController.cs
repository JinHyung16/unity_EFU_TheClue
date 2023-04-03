using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("GameSetUpData")]
    [SerializeField] private GameSetUpData gameSetUpData;

    private Rigidbody playerRigidbody;
    
    private Vector3 moveDirection;
    private Vector3 jumpDirection;

    private float moveSpeed;
    private float jumpPower;

    private bool isGround = false;

    private void Start()
    {
        if (gameSetUpData == null)
        {
            gameSetUpData = Resources.Load("Data/GameSetUpData") as GameSetUpData;
        }
        playerRigidbody = GetComponent<Rigidbody>();

        //bind player movement data
        this.moveSpeed = gameSetUpData.moveSpeed;
        this.jumpPower = gameSetUpData.jumpPower;
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGround = true;
        }
    }

    public void MoveDirection(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, 0, direction.z);
    }

    public void JumpDirection(Vector3 direction)
    {
        jumpDirection = direction;
    }

    private void Movement()
    {
        if (GameManager.GetInstance.IsInputStop || GameManager.GetInstance.IsUIOpen)
        {
            moveDirection *= 0;
        }
        playerRigidbody.velocity = moveDirection * moveSpeed;
    }

    private void Jump()
    {
        if (isGround)
        {
            playerRigidbody.AddForce(jumpDirection * jumpPower, ForceMode.Impulse);
            isGround = false;
        }
    }
}
