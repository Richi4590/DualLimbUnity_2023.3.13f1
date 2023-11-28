using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Adjust the speed as needed

    void Update()
    {
        // Rotate the object around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}