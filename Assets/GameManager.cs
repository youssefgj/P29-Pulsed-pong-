using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private AIPaddle aiPaddle;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;

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
        ResetPositions();

    }
    private void ResetPositions() 
    {
        ball.Reset();
        playerPaddle.Reset();
        aiPaddle.Reset();
    }
}
