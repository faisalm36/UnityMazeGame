using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float degreesPerSecond = 60f;

    void Update()
    {
        transform.Rotate(axis, degreesPerSecond * Time.deltaTime, Space.World);
    }
}
