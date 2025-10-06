using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 spinSpeed = new Vector3(0f, 120f, 0f);
    void Update() => transform.Rotate(spinSpeed * Time.deltaTime, Space.Self);
}

