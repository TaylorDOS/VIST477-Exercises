using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnInterval = 5f;
    public float spawnDistance = 10f;
    public float floatSpeed = 1f;
    public float maxHeight = 20f;

    private float nextSpawnTime;

    void Update()
    {
        
        // Check if it's time to spawn a balloon
        if (Time.time >= nextSpawnTime)
        {
            SpawnBalloon();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnBalloon()
    {
        float randomX = Random.Range(-10f, 10f);
        // Calculate a random position in front of the player
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
        // Ensure the balloon spawns at ground level
        spawnPosition.y = 0f;
        spawnPosition.x = randomX;

        // Instantiate the balloon at the spawn position
        GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

        // Set up the balloon's floating behavior
        BalloonMovement balloonMovement = balloon.GetComponent<BalloonMovement>();
        if (balloonMovement != null)
        {
            balloonMovement.floatSpeed = floatSpeed;
            balloonMovement.maxHeight = maxHeight;
        }
        else
        {
            Debug.LogWarning("Balloon prefab is missing the BalloonMovement component.");
        }
    }
}