using UnityEngine;
using UnityEngine.InputSystem; // <-- IMPORTANT: Add the using statement

public class Paddle : MonoBehaviour
{
    private Vector2 startPosition;
    private float targetX; // Variable to hold the current input position

    void Start()
    {
        startPosition = transform.position;
        targetX = startPosition.x;
    }

    // New method to handle the input action event
    public void OnMovePaddle(InputAction.CallbackContext context)
    {
        // Read the 2D value (touch or mouse position)
        if (context.performed)
        {
            // Get the screen position from the input action
            Vector2 screenPosition = context.ReadValue<Vector2>();

            // Convert screen position to world position and store the X component
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            targetX = worldPosition.x;
        }
        // Handle when input ends (optional, but good practice)
        else if (context.canceled)
        {
            // You might want to stop movement here if you weren't constantly tracking
        }
    }

    void Update()
    {
        // 1. Clamp the calculated X position to stay within the desired horizontal bounds
        float newX = Mathf.Clamp(targetX, -8.0f, 8.0f); // Adjust bounds as needed

        // 2. Apply the new position, keeping Y fixed
        transform.position = new Vector2(newX, transform.position.y);
    }

    public void Reset()
    {
        transform.position = startPosition;
    }
}