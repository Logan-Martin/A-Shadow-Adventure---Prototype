using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotSpeed = 50f;

    void Update()
    {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }
}
