using UnityEngine;

public class HeartRateManager : MonoBehaviour
{
    [Header("Dependencies")]
    public AudioSource backgroundMusic;

    [Header("Mode")]
    [Tooltip("If true, the slider below controls BPM. If false, the Camera controls it.")]
    public bool useSimulatedHeartRate = false;

    [Header("Heart Rate Data")]
    [Range(40, 160)]
    public float currentHeartRate = 60f; // Adjusted by Camera or Slider

    [Header("Your Physiology (The Bornes)")]
    public float minBPM = 50f;    // Resting (Game is Fast)
    public float maxBPM = 90f;    // Stressed (Game is Slow)

    [Header("Game Speed Limits")]
    public float fastestGameSpeed = 1.5f; // When you are Chill (50 BPM)
    public float slowestGameSpeed = 0.5f; // When you are Stressed (90 BPM)

    void Update()
    {
        if (Time.timeScale == 0) return; // Don't run if paused

        // 1. Convert Heart Rate to a 0.0 (Chill) - 1.0 (Stressed) scale
        // This math handles your custom 50-90 range automatically!
        float stressFactor = Mathf.InverseLerp(minBPM, maxBPM, currentHeartRate);

        // 2. Calculate Target Speed
        // Logic: Low Stress = Fast Speed, High Stress = Slow Speed
        float targetSpeed = Mathf.Lerp(fastestGameSpeed, slowestGameSpeed, stressFactor);

        // 3. Apply Smoothly
        // We use 'Unscaled' time so the transition happens smoothly even if game is slow
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetSpeed, Time.unscaledDeltaTime * 1.0f);

        // 4. Adjust Music Pitch
        if (backgroundMusic != null)
        {
            backgroundMusic.pitch = Time.timeScale;
        }
    }

    // This function allows the FingerPulseDetector to send data here safely
    public void SetHeartRate(float detectedBPM)
    {
        if (!useSimulatedHeartRate)
        {
            currentHeartRate = detectedBPM;
        }
    }
}