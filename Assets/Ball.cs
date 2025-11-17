using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        Launch();
    }
    public void Launch() 
    {
        rb.linearVelocity = new Vector2(GetRandomDirection(), GetRandomDirection());
    }
    public void Reset()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPosition;
        Launch();
    }
    private float GetRandomDirection() 
    {
        return (Random.Range(0, 1) == 0 ? -1 : 1) * speed;
    }
    
}
