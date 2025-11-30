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

    private int playerScore = 0;
    private int aiScore = 0;

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
}
