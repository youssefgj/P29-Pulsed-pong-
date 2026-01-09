using UnityEngine;

public class HeartRateManager : MonoBehaviour
{
    [Header("Dependencies")]
    public AudioSource backgroundMusic; // Drag your music source here

    [Header("Heart Rate Simulation")]
    [Range(40, 160)]
    public float currentHeartRate = 80f; // Slider to test manually!

    [Header("Difficulty Settings")]
    public float minBPM = 60f;    // Relaxed (Fast Game)
    public float maxBPM = 120f;   // Stressed (Slow Game)

    [Header("Game Speed Limits")]
    public float fastestGameSpeed = 1.5f;
    public float slowestGameSpeed = 0.5f;

    void Update()
    {
        // SAFETY CHECK: If the game is paused via UI, do not override it.
        if (Time.timeScale == 0) return;

        // 1. Convert Heart Rate to a 0.0 - 1.0 scale
        // 0.0 means at or below minBPM
        // 1.0 means at or above maxBPM
        float stressFactor = Mathf.InverseLerp(minBPM, maxBPM, currentHeartRate);

        // 2. Calculate Target Speed
        // If stress is 0 (Relaxed), target is fastestGameSpeed
        // If stress is 1 (Stressed), target is slowestGameSpeed
        float targetSpeed = Mathf.Lerp(fastestGameSpeed, slowestGameSpeed, stressFactor);

        // 3. Apply Smoothly (Linear Interpolation)
        // We use Time.unscaledDeltaTime because Time.deltaTime gets affected by the timeScale itself!
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetSpeed, Time.unscaledDeltaTime * 2f);

        // 4. Adjust Music Pitch to match Game Speed
        if (backgroundMusic != null)
        {
            backgroundMusic.pitch = Time.timeScale;
        }
    }
}