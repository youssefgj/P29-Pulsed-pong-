using UnityEngine;

public class Goal : MonoBehaviour
{
  [SerializeField] private GameManager gameManager;
  [SerializeField] private bool isPlayerGoal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            gameManager.PlayerGetScore(isPlayerGoal);
        }
    }

}
