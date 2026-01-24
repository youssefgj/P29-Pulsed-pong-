using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Serialized fields for UI components
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private AIPaddle aiPaddle;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;

    // Reference to the AudioSource component
    [SerializeField] private AudioSource goalSound;
    [SerializeField] private BackgroundMusicController musicManager;

    private int playerScore = 0;
    private int aiScore = 0;

    private bool isPaused = false;

    public void PlayerGetScore(bool isPlayer)
    {
        if(isPlayer)
        { 
            playerScore++;
            playerScoreText.text = playerScore.ToString();
        }
        else
        {
            aiScore++;
            aiScoreText.text = aiScore.ToString();
        }

        // Play sound when a goal is scored
        goalSound.Play();

        // Reset positions after the goal
        ResetPositions();
    }

    private void ResetPositions() 
    {
        ball.Reset();
        playerPaddle.Reset();
        aiPaddle.Reset();
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause game time
        AudioListener.pause = true;
    }
    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        AudioListener.pause = false;  // Resume audio
    }
}
