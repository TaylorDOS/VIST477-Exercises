using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float initialSpeed = 10f;
    public float launchAngle = 45f;

    void Start()
    {
        // Start shooting coroutine
        StartCoroutine(ShootProjectile());
    }

    IEnumerator ShootProjectile()
    {
        while (true)
        {
            // Calculate horizontal and vertical velocity components
            float horizontalVelocity = initialSpeed * Mathf.Cos(Mathf.Deg2Rad * launchAngle);
            float verticalVelocity = initialSpeed * Mathf.Sin(Mathf.Deg2Rad * launchAngle);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the rigidbody component of the projectile
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // Check if the rigidbody component exists
            if (rb != null)
            {
                // Set the initial velocity of the projectile
                rb.velocity = transform.forward * horizontalVelocity + transform.up * verticalVelocity;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }

            // Wait for 5 seconds before shooting again
            yield return new WaitForSeconds(5f);
        }
    }
}
