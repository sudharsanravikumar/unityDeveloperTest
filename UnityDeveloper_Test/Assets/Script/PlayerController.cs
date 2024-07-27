using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : GravityManipulation
{
    [Header("Player Settings")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float checkDistance = 1.1f; // Distance to check for ground
    [SerializeField] private float fallThreshold = -10f; // Speed threshold to consider the player is falling freely

    private bool isOnGround = false;
    public bool IsFalling { get; private set; } = false;
    private int cubeCount = 0;
    private GameManager gameManager;
    private GravityManipulation gravityManipulation;

    private Vector3 localGravity = Vector3.down * 9.81f; // Default gravity in local space

    private void Start()
    {
        // Initialize components
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        if (playerRb == null || playerAnim == null)
        {
            Debug.LogError("Required components are missing.");
        }

        // Find and set the GameManager
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found.");
        }
    }

    private void Update()
    {
        if (gameManager != null && !gameManager.IsGameOver())
        {
            MoveController();
            Jump();
            Fallout();
            CheckFreeFall();
            PlayerRotation();

            if (AllCubesCollected())
            {
                gameManager.ResetGame();
            }

            if (IsFalling)
            {
                gameManager.EndGame();
            }
        }

    }

    private void FixedUpdate()
    {
        ApplyLocalGravity();
    }

    private void ApplyLocalGravity()
    {
        if (!isOnGround)
        {
            playerRb.AddForce(localGravity, ForceMode.Acceleration);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Destroy(other.gameObject);
            cubeCount++;
        }
    }

    private bool AllCubesCollected()
    {
        const int totalCubes = 10; // Set your target number of cubes here
        return cubeCount >= totalCubes;
    }

    private void MoveController()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection = Vector3.forward;
        else if (Input.GetKey(KeyCode.S)) moveDirection = Vector3.back;
        else if (Input.GetKey(KeyCode.A)) moveDirection = Vector3.left;
        else if (Input.GetKey(KeyCode.D)) moveDirection = Vector3.right;

        transform.Translate(moveDirection * forwardSpeed * Time.deltaTime, Space.Self);
        playerAnim.SetFloat("Speed", moveDirection != Vector3.zero ? 1f : 0f, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    private void Fallout()
    {
        playerAnim.SetBool("Fall_Bool", !isOnGround);
    }

    private void CheckFreeFall()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, -localGravity.normalized, out hit, checkDistance);

        IsFalling = !isGrounded && playerRb.velocity.y < fallThreshold;
    }

    private void PlayerRotation()
    {
        // Ensure gravityManipulation.keyDirection is a valid direction vector
        if (gravityManipulation != null)
        {
            Vector3 direction = gravityManipulation.keyDirection;

            // Update player rotation based on keyDirection
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
