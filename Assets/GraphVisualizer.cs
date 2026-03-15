using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class GraphVisualizer : MonoBehaviour
{
    [Header("Container")]
    public RectTransform graphContainer;

    [Header("Visuals")]
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Color dotColor = new Color(1,1,1,1);
    [SerializeField] private Color lineColor = new Color(0,1,0,0.5f);
    [SerializeField] private float lineThickness = 3f;

    [Header("Labels & Grid")]
    [SerializeField] private GameObject labelTemplateX;
    [SerializeField] private GameObject labelTemplateY;
    [SerializeField] private GameObject dashTemplate;

    [Header("Session Menu")]
    [SerializeField] private GameObject sessionsPanel;

    private List<SessionData> lastSessions = new List<SessionData>();

    [SerializeField] private TextMeshProUGUI sessionDateText;

    void Start()
    {
        LoadLastSessions();

        if(lastSessions.Count > 0)
            ShowSession(0);
    }


    // -------------------------------
    // SESSION MENU
    // -------------------------------

    public void ToggleSessionsMenu()
    {
        if(sessionsPanel != null)
            sessionsPanel.SetActive(!sessionsPanel.activeSelf);
    }

    public void ShowSession(int index)
    {
        if(index < 0 || index >= lastSessions.Count)
        {
            Debug.LogWarning("Invalid session index.");
            return;
        }

        SessionData session = lastSessions[index];

        ShowGraph(session);

        if(sessionDateText != null)
        {
            sessionDateText.text = session.playDate;
        }

        if(sessionsPanel != null)
            sessionsPanel.SetActive(false);
    }


    // -------------------------------
    // LOAD LAST 3 SESSIONS
    // -------------------------------

    public void LoadLastSessions()
    {
        string path = Path.Combine(Application.persistentDataPath, "pulspong_sessions.json");

        if(!File.Exists(path))
        {
            Debug.LogError("Session file not found.");
            return;
        }

        string json = File.ReadAllText(path);

        if(string.IsNullOrEmpty(json))
        {
            Debug.LogError("Session file empty.");
            return;
        }

        SessionDatabase db = JsonUtility.FromJson<SessionDatabase>(json);

        if(db == null || db.allSessions.Count == 0)
        {
            Debug.LogWarning("No sessions found.");
            return;
        }

        lastSessions.Clear();

        int startIndex = Mathf.Max(0, db.allSessions.Count - 3);

        for(int i = db.allSessions.Count - 1; i >= startIndex; i--)
        {
            lastSessions.Add(db.allSessions[i]);
        }
    }


    // -------------------------------
    // DRAW GRAPH
    // -------------------------------

    public void ShowGraph(SessionData session)
    {
        // CLEAR EVERYTHING
        foreach(Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        if(session == null || session.heartRateLogs.Count < 2)
        {
            Debug.LogWarning("Not enough data.");
            return;
        }

        float yMax = 160f;
        float yMin = 40f;

        float xMax = session.heartRateLogs[session.heartRateLogs.Count - 1].timeStamp;

        if(xMax <= 0.1f)
            xMax = 1f;

        float graphHeight = graphContainer.rect.height;
        float graphWidth = graphContainer.rect.width;


        // -------------------------------
        // Y AXIS
        // -------------------------------

        int separatorCount = 5;

        for(int i = 0; i <= separatorCount; i++)
        {
            float normalizedValue = (float)i / separatorCount;
            float yPos = normalizedValue * graphHeight;

            if(labelTemplateY != null)
            {
                GameObject labelY = Instantiate(labelTemplateY, graphContainer);
                labelY.SetActive(true);

                float bpmValue = yMin + normalizedValue * (yMax - yMin);

                TextMeshProUGUI tmp = labelY.GetComponent<TextMeshProUGUI>();
                tmp.fontSize = 28;
                tmp.text = Mathf.RoundToInt(bpmValue).ToString();
                tmp.alignment = TextAlignmentOptions.Right;

                RectTransform rect = labelY.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.zero;
                rect.pivot = new Vector2(1f,0.5f);
                rect.sizeDelta = new Vector2(100,30);
                rect.anchoredPosition = new Vector2(-15f,yPos);
            }

            if(dashTemplate != null)
                CreateDash(new Vector2(0,yPos), new Vector2(graphWidth,yPos));
        }


        // -------------------------------
        // X AXIS
        // -------------------------------

        int xSeparatorCount = 6;

        for(int i = 0; i <= xSeparatorCount; i++)
        {
            float normalizedValue = (float)i / xSeparatorCount;
            float xPos = normalizedValue * graphWidth;

            if(labelTemplateX != null)
            {
                GameObject labelX = Instantiate(labelTemplateX, graphContainer);
                labelX.SetActive(true);

                float timeValue = normalizedValue * xMax;

                string timeText =
                    string.Format("{0}:{1:00}",
                    (int)timeValue / 60,
                    (int)timeValue % 60);

                TextMeshProUGUI tmp = labelX.GetComponent<TextMeshProUGUI>();
                tmp.fontSize = 28;
                tmp.text = timeText;

                RectTransform rect = labelX.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.zero;
                rect.pivot = new Vector2(0.5f,1f);
                rect.sizeDelta = new Vector2(100,30);
                rect.anchoredPosition = new Vector2(xPos,-15f);
            }
        }


        // -------------------------------
        // DRAW DATA
        // -------------------------------

        GameObject lastCircle = null;

        foreach(var log in session.heartRateLogs)
        {
            float xPos = (log.timeStamp / xMax) * graphWidth;
            float yPos = ((log.bpm - yMin)/(yMax-yMin)) * graphHeight;

            GameObject circle = CreateDot(new Vector2(xPos,yPos));

            if(lastCircle != null)
            {
                CreateConnection(
                    lastCircle.GetComponent<RectTransform>().anchoredPosition,
                    circle.GetComponent<RectTransform>().anchoredPosition
                );
            }

            lastCircle = circle;
        }
    }


    // -------------------------------
    // GRAPH OBJECTS
    // -------------------------------

    private GameObject CreateDot(Vector2 position)
    {
        GameObject obj = new GameObject("dot", typeof(Image));
        obj.transform.SetParent(graphContainer,false);

        Image img = obj.GetComponent<Image>();
        img.sprite = circleSprite;
        img.color = dotColor;

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(8,8);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;

        return obj;
    }


    private void CreateConnection(Vector2 a, Vector2 b)
    {
        GameObject obj = new GameObject("connection", typeof(Image));
        obj.transform.SetParent(graphContainer,false);

        obj.GetComponent<Image>().color = lineColor;

        RectTransform rect = obj.GetComponent<RectTransform>();

        Vector2 dir = (b - a).normalized;
        float dist = Vector2.Distance(a,b);

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.sizeDelta = new Vector2(dist + 2f,lineThickness);
        rect.anchoredPosition = a + dir * dist * .5f;

        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        rect.localEulerAngles = new Vector3(0,0,angle);
    }


    private void CreateDash(Vector2 start, Vector2 end)
    {
        GameObject obj = Instantiate(dashTemplate,graphContainer);
        obj.SetActive(true);

        RectTransform rect = obj.GetComponent<RectTransform>();

        Vector2 dir = (end-start).normalized;
        float dist = Vector2.Distance(start,end);

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.sizeDelta = new Vector2(dist,1f);
        rect.anchoredPosition = start + dir * dist * .5f;

        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        rect.localEulerAngles = new Vector3(0,0,angle);
    }
}