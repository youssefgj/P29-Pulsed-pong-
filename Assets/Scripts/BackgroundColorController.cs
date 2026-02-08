using UnityEngine;
using TMPro;

public class BackgroundColorController : MonoBehaviour
{
    [Header("Background Colors")]
    public Color relaxedColor = new Color(1f, 0.78f, 0.65f);   // Warm Relax
    public Color normalColor  = new Color(0.365f, 0.804f, 0.949f); // Soft Neutral
    public Color tenseColor   = new Color(0.65f, 0.85f, 1f);    // Soft Cool Tense

    [Header("References")]
    public PersonStateController stateController;  // Source of the current state
    // public TextMeshProUGUI stateLabel;            // Text in the middle

    [Header("Settings")]
    public float changeSpeed = 2f;                // Lerp speed

    private Camera cam;
    private Color targetColor;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        // Initialize visuals based on the starting state
        UpdateVisualsFromState();
        cam.backgroundColor = targetColor;
    }

    void Update()
    {
        // Every frame, make sure visuals match the current state
        UpdateVisualsFromState();

        cam.backgroundColor = Color.Lerp(
            cam.backgroundColor,
            targetColor,
            Time.deltaTime * changeSpeed
        );
        // Debug.Log(stateController.CurrentState + "  HR=" + FindObjectOfType<HeartRateManager>().currentHeartRate);

    }

    // Reads state from stateController and sets color + text
    void UpdateVisualsFromState()
    {
        if (stateController == null)
            return;

        switch (stateController.CurrentState)
        {
            case PersonState.Relax:
                targetColor = relaxedColor;
                // if (stateLabel != null) stateLabel.text = "Relax";
                break;

            case PersonState.Normal:
                targetColor = normalColor;
                // if (stateLabel != null) stateLabel.text = "Normal";
                break;

            case PersonState.Tense:
                targetColor = tenseColor;
                // if (stateLabel != null) stateLabel.text = "Tense";
                break;
        }
    }
}
