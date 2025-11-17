using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform ball;
    [SerializeField] private float speed = 2f;
    private Vector2 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Mathf.MoveTowards(transform.position.x, ball.position.x, speed * Time.deltaTime);
        transform.position = new Vector2(newX, startPosition.y);
        
    }
    public void Reset()
    {
        transform.position = startPosition;
    }
}
