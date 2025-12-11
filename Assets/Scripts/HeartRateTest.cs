using UnityEngine;

public class HeartRateTest : MonoBehaviour
{
    public PersonStateController stateController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Simulate relaxed heart rate
            stateController.UpdateStateFromHeartRate(60f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Simulate normal heart rate
            stateController.UpdateStateFromHeartRate(85f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Simulate tense heart rate
            stateController.UpdateStateFromHeartRate(120f);
        }
    }
}
