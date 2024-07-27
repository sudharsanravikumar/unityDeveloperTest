using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform parent;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float yMouseSensitivity = 100f;
    private GameManager gameManager;
    private bool isCursorLocked = true; // Track cursor lock state

    void Start()
    {
        parent = transform.parent;
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found.");
        }
        SetCursorState(true); // Lock the cursor at the start of the game
    }

    void Update()
    {
        if (gameManager != null && !gameManager.IsGameOver())
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        parent.Rotate(Vector3.up, mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * yMouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.right, -mouseY); // Inverted Y-axis rotation
    }

    public void SetCursorState(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
        isCursorLocked = isLocked;
    }
}
