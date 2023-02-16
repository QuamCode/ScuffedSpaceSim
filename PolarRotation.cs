using UnityEngine;

public class PolarRotation : MonoBehaviour
{
    public float rotationSpeed;     // The speed at which the sprite rotates

    void Update()
    {
        // Rotate the sprite around its own Z-axis at the rotation speed over time
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
