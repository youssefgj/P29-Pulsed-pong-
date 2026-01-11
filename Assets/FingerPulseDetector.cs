using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class FingerPulseDetector : MonoBehaviour
{
    [Header("UI References")]
    public RawImage cameraDisplay;
    public TMP_Text bpmText;

    // Private processing variables
    private WebCamTexture webcamTexture;
    private Color32[] pixelData;
    private List<float> rednessHistory = new List<float>();
    private float timer = 0f;

    // Reference to the manager
    private HeartRateManager heartRateManager;

    void Start()
    {
        // SAFER CONNECTION: Finds the manager even if it's on a different object
        heartRateManager = FindObjectOfType<HeartRateManager>();

        // --- 1. FIND THE BACK CAMERA ---
        WebCamDevice[] devices = WebCamTexture.devices;
        string backCamName = "";
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing) { backCamName = devices[i].name; break; }
        }

        // --- 2. START CAMERA ---
        if (backCamName != "") webcamTexture = new WebCamTexture(backCamName, 320, 240, 30);
        else webcamTexture = new WebCamTexture(320, 240, 30);

        webcamTexture.Play();

        if (cameraDisplay != null)
        {
            cameraDisplay.texture = webcamTexture;
            cameraDisplay.rectTransform.localEulerAngles = new Vector3(0, 0, -webcamTexture.videoRotationAngle);
        }

        pixelData = new Color32[webcamTexture.width * webcamTexture.height];
    }

    void Update()
    {
        if (webcamTexture == null || !webcamTexture.didUpdateThisFrame) return;

        float avgRed = GetAverageRed();

        rednessHistory.Add(avgRed);
        if (rednessHistory.Count > 150) rednessHistory.RemoveAt(0);

        timer += Time.unscaledDeltaTime;
        if (timer > 0.2f)
        {
            CalculateBPM();
            timer = 0;
        }
    }

    float GetAverageRed()
    {
        webcamTexture.GetPixels32(pixelData);
        long sum = 0;
        int count = 0;
        // Optimization: Sample every 5th pixel
        for (int i = 0; i < pixelData.Length; i += 5)
        {
            sum += pixelData[i].r;
            count++;
        }
        return (float)sum / count;
    }

    void CalculateBPM()
    {
        if (rednessHistory.Count < 30) return;

        float average = rednessHistory.Average();
        int peaks = 0;
        bool isBeat = false;
        float threshold = 0.5f; // Adjust this if detection is too sensitive

        for (int i = 1; i < rednessHistory.Count; i++)
        {
            if (rednessHistory[i] > average + threshold && !isBeat)
            {
                isBeat = true;
                peaks++;
            }
            else if (rednessHistory[i] < average)
            {
                isBeat = false;
            }
        }

        float timeWindow = rednessHistory.Count / 30f;
        float rawBPM = (peaks / timeWindow) * 60f;

        // Clamp to logical limits
        rawBPM = Mathf.Clamp(rawBPM, 45, 160);

        // Send the data to the Manager
        if (heartRateManager != null)
        {
            // We smooth the value before sending so the game speed doesn't jitter
            float smoothBPM = Mathf.Lerp(heartRateManager.currentHeartRate, rawBPM, 0.2f);

            // SEND IT!
            heartRateManager.SetHeartRate(smoothBPM);

            if (bpmText != null) bpmText.text = "BPM: " + Mathf.Round(smoothBPM);
        }
    }
}