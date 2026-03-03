using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO; // IMPORTANT: Needed for File reading
using System.Collections.Generic;

public class GraphVisualizer : MonoBehaviour
{
    [Header("Container")]
    public RectTransform graphContainer;

    [Header("Visuals")]
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Color dotColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color lineColor = new Color(0, 1, 0, 0.5f);
    [SerializeField] private float lineThickness = 3f;

    [Header("Labels & Grid")]
    [SerializeField] private GameObject labelTemplateX;
    [SerializeField] private GameObject labelTemplateY;
    [SerializeField] private GameObject dashTemplate;

    // --- THIS WAS MISSING: The Trigger ---
    private void Start()
    {
        LoadLastSession();
    }

    // --- THIS WAS MISSING: The Loader ---
    public void LoadLastSession()
    {
        string path = Path.Combine(Application.persistentDataPath, "pulspong_sessions.json");

        Debug.Log("Attempting to load data from: " + path);

        if (!File.Exists(path))
        {
            Debug.LogError("File not found! Play the game first to generate data.");
            return;
        }

        string json = File.ReadAllText(path);

        // Safety check if file is empty
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("File is empty.");
            return;
        }

        SessionDatabase db = JsonUtility.FromJson<SessionDatabase>(json);

        if (db != null && db.allSessions.Count > 0)
        {
            // Get the most recent session
            SessionData lastSession = db.allSessions[db.allSessions.Count - 1];
            ShowGraph(lastSession);
        }
        else
        {
            Debug.LogWarning("Database loaded but no sessions found inside.");
        }
    }

    // --- The Drawing Logic ---
    public void ShowGraph(SessionData session)
    {
        // 1. Clean up previous graph
        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        if (session == null || session.heartRateLogs.Count < 2)
        {
            Debug.LogWarning("GraphVisualizer: Not enough data.");
            return;
        }

        // 2. Define Axis Limits
        float yMax = 160f;
        float yMin = 40f;
        float xMax = session.heartRateLogs[session.heartRateLogs.Count - 1].timeStamp;
        if (xMax <= 0.1f) xMax = 1f;

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        // 3. Draw Grid & Y-Axis Labels (FIXED ALIGNMENT)
        int separatorCount = 5;
        for (int i = 0; i <= separatorCount; i++)
        {
            float normalizedValue = (float)i / separatorCount;
            float yPos = normalizedValue * graphHeight;

            if (labelTemplateY != null)
            {
                GameObject labelY = Instantiate(labelTemplateY, graphContainer);
                labelY.SetActive(true);

                // Set Value
                float bpmValue = yMin + (normalizedValue * (yMax - yMin));
                TextMeshProUGUI tmp = labelY.GetComponent<TextMeshProUGUI>();
                tmp.text = Mathf.RoundToInt(bpmValue).ToString();

                // FORCE ALIGNMENT: Align Text to the Right
                tmp.alignment = TextAlignmentOptions.Right;

                // FORCE POSITION: Pivot on the Right edge (1, 0.5)
                RectTransform labelRect = labelY.GetComponent<RectTransform>();
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.zero;
                labelRect.pivot = new Vector2(1f, 0.5f); // Pivot on the Right Edge
                labelRect.sizeDelta = new Vector2(100, 30); // Give it space to render

                // Position it 15 pixels to the LEFT of the axis
                labelRect.anchoredPosition = new Vector2(-15f, yPos);
                labelRect.localScale = Vector3.one;
            }

            if (dashTemplate != null) CreateDash(new Vector2(0, yPos), new Vector2(graphWidth, yPos));
        }

        // 4. Draw X-Axis Labels (FIXED ALIGNMENT)
        int xSeparatorCount = 6;
        for (int i = 0; i <= xSeparatorCount; i++)
        {
            float normalizedValue = (float)i / xSeparatorCount;
            float xPos = normalizedValue * graphWidth;

            if (labelTemplateX != null)
            {
                GameObject labelX = Instantiate(labelTemplateX, graphContainer);
                labelX.SetActive(true);

                // Set Value
                float timeValue = normalizedValue * xMax;
                string timeText = string.Format("{0}:{1:00}", (int)timeValue / 60, (int)timeValue % 60);

                TextMeshProUGUI tmp = labelX.GetComponent<TextMeshProUGUI>();
                tmp.text = timeText;

                // FORCE ALIGNMENT: Center the text
                tmp.alignment = TextAlignmentOptions.Top;

                // FORCE POSITION: Pivot on the Top Edge (0.5, 1)
                RectTransform labelRect = labelX.GetComponent<RectTransform>();
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.zero;
                labelRect.pivot = new Vector2(0.5f, 1f); // Pivot on Top
                labelRect.sizeDelta = new Vector2(100, 30);

                // Position it 15 pixels BELOW the axis
                labelRect.anchoredPosition = new Vector2(xPos, -15f);
                labelRect.localScale = Vector3.one;
            }
        }

        // 5. Draw the Graph Data
        GameObject lastCircle = null;
        for (int i = 0; i < session.heartRateLogs.Count; i++)
        {
            float xPos = (session.heartRateLogs[i].timeStamp / xMax) * graphWidth;
            float yPos = ((session.heartRateLogs[i].bpm - yMin) / (yMax - yMin)) * graphHeight;

            GameObject currentCircle = CreateDot(new Vector2(xPos, yPos));

            if (lastCircle != null)
            {
                CreateConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition,
                                 currentCircle.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircle = currentCircle;
        }
    }

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = dotColor;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(8, 8);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void CreateConnection(Vector2 dotA, Vector2 dotB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = lineColor;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotB - dotA).normalized;
        float distance = Vector2.Distance(dotA, dotB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance + 2f, lineThickness);
        rectTransform.anchoredPosition = dotA + dir * distance * 0.5f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void CreateDash(Vector2 start, Vector2 end)
    {
        GameObject gameObject = Instantiate(dashTemplate, graphContainer);
        gameObject.SetActive(true);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1f);
        rectTransform.anchoredPosition = start + dir * distance * 0.5f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}