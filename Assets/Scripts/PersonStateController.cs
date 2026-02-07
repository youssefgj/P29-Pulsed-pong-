using UnityEngine;

public enum PersonState
{
    Relax,
    Normal,
    Tense
}

public class PersonStateController : MonoBehaviour
{
    // Current state of the person based on heart rate
    public PersonState CurrentState { get; private set; } = PersonState.Normal;

    // Decide the state only from heart rate value
    public void UpdateStateFromHeartRate(float heartRate)
    {
        if (heartRate < 60f)
            CurrentState = PersonState.Relax;
        else if (heartRate <= 100f)
            CurrentState = PersonState.Normal;
        else
            CurrentState = PersonState.Tense;
    }
}
