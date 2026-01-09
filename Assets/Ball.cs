using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float spawnHeightRange = 3f;

    [Header("Hit Feel Settings")] // New section for the "Juice"
    [SerializeField] private float speedMultiplier = 1.1f; // 1.1 = 10% faster per hit
    [SerializeField] private float maxSpeed = 15f;         // Cap to prevent glitches

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        rb.linearVelocity = Vector2.zero;

        // Reset speed to base level in case it got fast in the last round
        // (Optional, but good practice if you modify 'speed' variable directly)

        float randomY = Random.Range(-spawnHeightRange, spawnHeightRange);
        transform.position = new Vector2(0, randomY);

        StartCoroutine(LaunchAfterDelay());
    }

    private IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Launch();
    }

    public void Launch()
    {
        float xDir = Random.Range(0, 2) == 0 ? -1 : 1;
        float yDir = Random.Range(0.2f, 1.0f);
        if (Random.Range(0, 2) == 0) yDir = -yDir;

        Vector2 direction = new Vector2(xDir, yDir);
        rb.linearVelocity = direction.normalized * speed;
    }

    // --- NEW CODE BELOW ---

    // This function runs automatically whenever the ball hits a collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit the Paddle (Make sure your paddles have the tag "Player")
        // Or if you want it to speed up on walls too, remove the 'if' check.
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. Multiply the current velocity
            Vector2 newVelocity = rb.linearVelocity * speedMultiplier;

            // 2. Cap the speed so it doesn't break the game
            rb.linearVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);

            // Optional: Debug log to see the speed increase in Console
            // Debug.Log("Hit! New Speed: " + rb.linearVelocity.magnitude);
        }
    }
}