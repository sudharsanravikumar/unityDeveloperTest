using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public Button restartButton;
    public GameObject gameOverScreen;

    [Header("Game Settings")]
    [SerializeField] private float timeLimit = 120f; // 2 minutes in seconds

    private float timeRemaining;
    private bool isGameOver = false;
    private PlayerController playerController;
    private CameraController cameraController;

    void Start()
    {
        timeRemaining = timeLimit;
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>(); // Find the CameraController in the scene

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
        if (cameraController == null)
        {
            Debug.LogError("CameraController not found in the scene.");
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(ResetGame);
        }
        else
        {
            Debug.LogError("Restart Button is not assigned.");
        }

        gameOverScreen.SetActive(false);
        if (cameraController != null)
        {
            cameraController.SetCursorState(true); // Lock cursor at the start
        }
    }

    void Update()
    {
        if (!isGameOver && playerController != null && !playerController.IsFalling)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                EndGame();
            }
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Countdown: {minutes:00}:{seconds:00}";
    }

    public void EndGame()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
        if (cameraController != null)
        {
            cameraController.SetCursorState(false); // Unlock cursor when game ends
        }
        Debug.Log("Game Over");
    }

    public void ResetGame()
    {
        StartCoroutine(RestartAfterDelay(0.5f));
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        timeRemaining = timeLimit;
        isGameOver = false;
        UpdateTimerDisplay();
        gameOverScreen.SetActive(false);
        if (cameraController != null)
        {
            cameraController.SetCursorState(true); // Lock cursor again when restarting
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
