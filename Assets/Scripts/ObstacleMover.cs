using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public Vector3 moveAxis = Vector3.right; // direction to move
    public float distance = 3f;              // total travel range
    public float speed = 1.5f;               // how fast it moves

    private Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * (distance * 0.5f);
        transform.position = startPos + moveAxis.normalized * offset;
    }
}
