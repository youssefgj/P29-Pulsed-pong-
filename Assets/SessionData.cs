using System;
using System.Collections.Generic;

[Serializable]
public class SessionData
{
    public string sessionId;
    public string playDate;     // Stored as string for JSON compatibility
    public float duration;
    public List<HeartRatePoint> heartRateLogs = new List<HeartRatePoint>();
}

[Serializable]
public struct HeartRatePoint
{
    public float timeStamp; // Time in seconds since game start
    public float bpm;       // The heart rate value
}

// Wrapper to help Unity's JsonUtility save a list of sessions
[Serializable]
public class SessionDatabase
{
    public List<SessionData> allSessions = new List<SessionData>();
}