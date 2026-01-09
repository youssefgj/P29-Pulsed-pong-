using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    private Vector2 startPosition;
    private float targetX;

    // We need to know how wide the paddle is to stop it correctly
    private float halfWidth;

    // Define your wall positions here (or calculated from screen)
    [SerializeField] private float leftWallX = -8.0f;
    [SerializeField] private float rightWallX = 8.0f;

    void Start()
    {
        startPosition = transform.position;
        targetX = startPosition.x;

        // AUTOMATICALLY CALCULATE PADDLE WIDTH
        // This gets the distance from the center to the edge.
        // It works even if you resize the paddle later!
        if (GetComponent<SpriteRenderer>() != null)
        {
            halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        }
        else
        {
            // Fallback if no SpriteRenderer (e.g., standard Cube)
            halfWidth = transform.localScale.x / 2f;
        }
    }

    public void OnMovePaddle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 screenPosition = context.ReadValue<Vector2>();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            targetX = worldPosition.x;
        }
    }

    void Update()
    {
        // THE FIX IS HERE:
        // We clamp between (LeftWall + Width) and (RightWall - Width)
        float clampedX = Mathf.Clamp(targetX, leftWallX + halfWidth, rightWallX - halfWidth);

        transform.position = new Vector2(clampedX, transform.position.y);
    }

    public void Reset()
    {
        transform.position = startPosition;
        targetX = startPosition.x; // Don't forget to reset the target too!
    }
}