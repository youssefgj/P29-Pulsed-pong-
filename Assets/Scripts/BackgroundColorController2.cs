using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BackgroundColorController2 : MonoBehaviour
{
    public HeartRateManager heartRateManager;
    // Low heart rate
    public float minHR = 40f;
    // High heart rate
    public float maxHR = 120f; 
    //bright
    public Color lowColor = Color.white;
    // Dark blue
    public Color highColor = new Color32(0, 3, 10, 255);

    public float changeSpeed = 2f;

    Camera cam;

    void Awake() => cam = GetComponent<Camera>();

    void Update()
    {
        if (!heartRateManager) return;

        // Inverted mapping
        float t = 1f - Mathf.InverseLerp(minHR, maxHR, heartRateManager.currentHeartRate);

        cam.backgroundColor = Color.Lerp(
            cam.backgroundColor,
            Color.Lerp(lowColor, highColor, t),
            Time.deltaTime * changeSpeed
        );
    }
}