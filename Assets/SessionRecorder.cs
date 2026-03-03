using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SessionRecorder : MonoBehaviour
{
    private HeartRateManager hrManager;
    private SessionData currentSession;
    private float startTime;
    private string saveFilePath;

    void Start()
    {
        hrManager = FindObjectOfType<HeartRateManager>();

        // Define where to save (works on PC and Mobile)
        saveFilePath = Path.Combine(Application.persistentDataPath, "pulspong_sessions.json");

        // Initialize new session
        currentSession = new SessionData();
        currentSession.sessionId = Guid.NewGuid().ToString(); // Unique ID
        currentSession.playDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        startTime = Time.time;

        // Start recording loop
        StartCoroutine(RecordRoutine());
    }

    IEnumerator RecordRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f); // Record every 1 second

            if (hrManager != null)
            {
                HeartRatePoint point = new HeartRatePoint();
                point.timeStamp = Time.time - startTime;
                point.bpm = hrManager.currentHeartRate;

                currentSession.heartRateLogs.Add(point);
            }
        }
    }

    // This is called automatically when the Scene is closed (Game Over or Quit)
    void OnDestroy()
    {
        SaveSessionToDisk();
    }

    void SaveSessionToDisk()
    {
        currentSession.duration = Time.time - startTime;

        // 1. Load existing database
        SessionDatabase database = new SessionDatabase();
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            database = JsonUtility.FromJson<SessionDatabase>(json);
        }

        // 2. Add current session
        database.allSessions.Add(currentSession);

        // 3. Write back to disk
        string newJson = JsonUtility.ToJson(database, true); // 'true' makes it readable
        File.WriteAllText(saveFilePath, newJson);

        Debug.Log($"Session Saved! Path: {saveFilePath}");
    }
}