using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    public float rotationSpeed = 20f;

    public float moveSpeed = 5f;

    private Vector3 startPosition;
    private float zDirection = 1f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        float yMovement = zDirection * moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * yMovement);

        float currentHeight = transform.position.y - startPosition.y;

        if (Mathf.Abs(currentHeight) > 3f)
            zDirection *= -1f;
    }
}
