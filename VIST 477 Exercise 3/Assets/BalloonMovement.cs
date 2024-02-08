using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float maxHeight = 20f;

    void Update()
    {
        // Move the balloon upwards
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // Check if the balloon has reached the maximum height
        if (transform.position.y >= maxHeight)
        {
            // Destroy the balloon
            Destroy(gameObject);
        }
    }
}