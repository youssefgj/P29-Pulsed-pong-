using System.Collections; // Needed for IEnumerator
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float spawnHeightRange = 3f; // New: How much up/down it can spawn

    // We don't need startPosition anymore if we are randomizing it

    void Start()
    {
        Reset(); // Call Reset immediately to start the loop
    }

    public void Reset()
    {
        // 1. Stop the ball
        rb.linearVelocity = Vector2.zero;

        // 2. Randomize Y Position (Fixes "Same Position" problem)
        // Keep X at 0, but pick a random Y
        float randomY = Random.Range(-spawnHeightRange, spawnHeightRange);
        transform.position = new Vector2(0, randomY);

        // 3. Wait before launching (Better gameplay flow)
        StartCoroutine(LaunchAfterDelay());
    }

    private IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Waits 1 second
        Launch();
    }

    public void Launch()
    {
        // Fix 1: Randomize X Direction correctly
        // Random.Range(0, 2) gives either 0 or 1.
        float xDir = Random.Range(0, 2) == 0 ? -1 : 1;

        // Fix 2: Randomize Y Direction (Angle)
        // This ensures the ball doesn't always move at 45 degrees.
        // We pick a value between 0.2 and 1.0 to avoid boring straight lines.
        float yDir = Random.Range(0.2f, 1.0f);

        // Randomly flip Y to be up or down
        if (Random.Range(0, 2) == 0) yDir = -yDir;

        Vector2 direction = new Vector2(xDir, yDir);

        // Fix 3: Normalize!
        // This ensures the speed is exactly "speed", no matter the angle.
        rb.linearVelocity = direction.normalized * speed;
    }
}