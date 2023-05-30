using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("GameSetUpData")]
    [SerializeField] private GameSetUpData gameSetUpData;

    private Rigidbody playerRigidbody;
    
    private Vector3 moveDirection;

    private float moveSpeed;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        //bind player movement data
        this.moveSpeed = gameSetUpData.moveSpeed;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void MoveDirection(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, 0, direction.z);
    }

    private void Movement()
    {
        if (GameManager.GetInstance.IsInputStop || GameManager.GetInstance.IsUIOpen)
        {
            moveDirection *= 0;
        }
        playerRigidbody.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}
