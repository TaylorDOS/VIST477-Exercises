using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody))]


public class RB_Walker : MonoBehaviour
{
    public Camera playerCamera;
    public float speed = 20.0f;
    public GameObject hmdUI;
    public GameObject map;
    private InputData inputData;
    private TaggingMechanism updateUI;
    private PhotonView myView;
    private Rigidbody r;
    private float originalSpeed;
    private bool isBoosted = false, RadarOn = false;
    private float totalBoostTime = 0.0f, RadarTime = 0.0f;
    private float elapsedTime = 0.0f;
    private int teleportCredits = 0;
    private Vector2 rotation = Vector2.zero;
    private float maxVelocityChange = 10.0f;
    private float xInput;
    private float yInput;
    private bool isChaser;
    private int repulsorPowerUpCount = 0;

    void Awake()
    {
        inputData = GetComponent<InputData>();
        myView = GetComponent<PhotonView>();
        r = GetComponent<Rigidbody>();
        originalSpeed = speed;
        r.freezeRotation = true;
        r.useGravity = false;
        r.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rotation.y = transform.eulerAngles.y;
        // Start camera and canvas only to myself in PUN
        if (myView.IsMine)
        {
            Transform childTransform = transform.GetChild(0);
            if (childTransform != null)
            {
                GameObject child = childTransform.gameObject;
                child.SetActive(true);
            }
            if (hmdUI != null)
            {
                hmdUI.GetComponent<Canvas>().enabled = true;
            }
        }
    }
    void Update()
    {
        if(myView.IsMine)
        {
            // movement input from controller
            if(inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
            {
                xInput = movement.x;
                yInput = movement.y;
            }
            // movement input from PC
            else{
                xInput = Input.GetAxis("Horizontal");
                yInput = Input.GetAxis("Vertical");
            }

            // use teleport powerup from controller
            if (inputData.rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                if (triggerValue > 0.5f)
                {
                    // Check if the player has teleport credits and perform teleportation
                    if (HasTeleportCredit())
                    {
                        UseTeleportCredit();
                        PerformTeleportation();
                    }
                    else
                    {
                        Debug.Log("No teleport credits available!");
                        updateUI.UpdatePowerUpUI("Collect new PowerUp");
                    }
                }
            }

            // use teleport powerup from PC
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Check if the player has teleport credits and perform teleportation
                if (HasTeleportCredit())
                {
                    UseTeleportCredit();
                    PerformTeleportation();
                }
                else
                {
                    Debug.Log("No teleport credits available!");
                    updateUI.UpdatePowerUpUI("Collect new PowerUp");
                }
            }

            // use attractor/repulsor powerup fron controller
            if (inputData.rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButtonPressed))
            {
                if (gripButtonPressed)
                {
                    if (repulsorPowerUpCount > 0)
                    {
                        UseRepulsorPowerUp();
                    }
                    else
                    {
                        Debug.Log("No repulsive credits available!");
                        updateUI.UpdatePowerUpUI("Collect new PowerUp");
                    }
                }
            }

            // use attractor/repulsor powerup fron PC
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (repulsorPowerUpCount>0)
                {
                    UseRepulsorPowerUp();
                }
                else
                {
                    Debug.Log("No repulsive credits available!");
                    updateUI.UpdatePowerUpUI("Collect new PowerUp");
                }
            }

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        updateUI = GetComponent<TaggingMechanism>();
        if (collision.gameObject.CompareTag("Boost"))
        {
            if (!isBoosted)
            {
                totalBoostTime = 5.0f;
                // Increase speed when colliding with a boost power-up
                StartCoroutine(BoostSpeed(20.0f, 40.0f, 5.0f));
                updateUI.UpdatePowerUpUI("Boost");
                Debug.Log("Collision with Cube Detected! Speed increased to: " + speed);
                Destroy(collision.gameObject);
            }
            else
            {
                elapsedTime = 0.0f;
                //totalBoostTime += 5.0f;
                // If a new boost is encountered while the timer is still active, destroy the new boost
                Debug.Log("Already boosted. Ignoring new boost power-up.");
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Teleportation"))
        {
            GrantTeleportCredit();
            Debug.Log("Teleportation Credit Obtained!");
            updateUI.UpdatePowerUpUI("Teleport");
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Repulsor"))
        {
            CollectRepulsorPowerUp();
            Debug.Log("Repulsor Power-Up Obtained!");
            updateUI.UpdatePowerUpUI("Repulsor");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Radar"))
        {
            
              
                
            
             if (!RadarOn)
            {
                RadarTime= 10.0f;
                // Increase speed when colliding with a boost power-up
                StartCoroutine(RadarTimer(RadarTime));
                updateUI.UpdatePowerUpUI("Radar");
               
                Destroy(collision.gameObject);
            }
            else
            {
                RadarTime = 10.0f;
                
                Destroy(collision.gameObject);
            }
            
            Debug.Log("Radar Power-Up Obtained!");
            
            

            Destroy(collision.gameObject);
        }
    }

    IEnumerator RadarTimer(float duration)
    {
        RadarOn = true;
        float Rtime = duration;
       
    
        map.SetActive(true);
        while (Rtime>0)
        {
            // Check for collisions during the boost duration
            yield return null;
            Rtime -= Time.deltaTime;
            
        }

        // Reset speed and flags when the boost duration is over
        map.SetActive(false);
         RadarOn = false;
        RadarTime = 0.0f; // Reset total boost time
        updateUI.UpdatePowerUpUI("Radar Ended");
           
    }


    void CollectRepulsorPowerUp()
    {
        // Increment repulsor power-up count when collected
        repulsorPowerUpCount++;
    }
    void GrantTeleportCredit()
    {
        // Increment teleport credits when a teleportation cube is collected
        teleportCredits++;
    }

    IEnumerator BoostSpeed(float boostAmount, float boostSpeed, float duration)
    {
        isBoosted = true;
        speed = Mathf.Min(speed + boostAmount, boostSpeed);

        while (elapsedTime < totalBoostTime)
        {
            // Check for collisions during the boost duration
            yield return null;
            elapsedTime += Time.deltaTime;
            
        }

        // Reset speed and flags when the boost duration is over
        speed = originalSpeed;
        isBoosted = false;
        totalBoostTime = 0.0f; // Reset total boost time
        elapsedTime = 0.0f;
        Debug.Log("Speed reverted back to: " + speed);
        updateUI.UpdatePowerUpUI("Collect new PowerUp");
    }

    

    void UseRepulsorPowerUp()
    {
        // Use repulsor power-up
        Debug.Log("Using Repulsor Power-Up.");
        updateUI.UpdatePowerUpUI("Using Repulsor PowerUp");

        GameObject[] opponents;
        if (gameObject.CompareTag("Runner")){
            opponents = GameObject.FindGameObjectsWithTag("Tagger");
            isChaser = true;
            Debug.Log("It's runner");
        }
        else{
            opponents = GameObject.FindGameObjectsWithTag("Runner");
            isChaser = false;
            Debug.Log("It's tagger");
        }
        // Find all opponents in the scene (you might need to adjust this based on your setup)
        

        // Define the duration of the repulsion effect
        float repulsionDuration = 5.0f;

        foreach (var opponent in opponents)
        {
            // Calculate the distance between the player and the opponent
            float distance = Vector3.Distance(transform.position, opponent.transform.position);

            // Check if the opponent is within the specified range
            if (distance < 20.0f)
            {
                // Calculate the force direction based on whether the player is a chaser or a runner

                Vector3 forceDirection = isChaser ? (opponent.transform.position - transform.position).normalized : (transform.position - opponent.transform.position).normalized;

                // Apply a force to attract or repulse the opponent over the specified duration
                float repulsorForce = 30.0f; // Adjust the force as needed
                StartCoroutine(ApplyRepulsionForce(opponent.GetComponent<Rigidbody>(), forceDirection, repulsorForce, repulsionDuration));
            }
        }

        // Reduce the repulsor power-up count
        repulsorPowerUpCount--;
    }

    IEnumerator ApplyRepulsionForce(Rigidbody opponentRigidbody, Vector3 forceDirection, float forceStrength, float duration)
    {
        // Apply the force over the specified duration
        float elapsedTime = 0.0f;
        Debug.Log("Repulsive/attarctive power up started");
        while (elapsedTime < duration)
        {
            
            opponentRigidbody.AddForce(forceDirection * forceStrength, ForceMode.Force);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        Debug.Log("Repulsive/attarctive power up ended");
    }

    bool HasTeleportCredit()
    {
        // Check if the player has teleport credits
        return teleportCredits > 0;
    }

    void UseTeleportCredit()
    {
        // Use teleport credit
        Debug.Log("You are using your teleportation credit.");
        teleportCredits--;
    }

    void PerformTeleportation()
    {
        // Teleport the player a certain distance forward
        float teleportDistance = 15.0f; // Adjust teleport distance based on your requirements

        // Calculate the target position
        Vector3 targetPosition = transform.position + transform.forward * teleportDistance;

        // Use Rigidbody's MovePosition to set the new position
        r.MovePosition(targetPosition);
    }

    void FixedUpdate()
    {
        // Calculate how fast we should be moving
        Vector3 forwardDir = Vector3.Cross(transform.up, -playerCamera.transform.right).normalized;
        Vector3 rightDir = Vector3.Cross(transform.up, playerCamera.transform.forward).normalized;
        Vector3 targetVelocity = forwardDir * yInput * speed + rightDir * xInput * speed;
        Vector3 velocity = transform.InverseTransformDirection(r.velocity);
        velocity.y = 0;
        velocity = transform.TransformDirection(velocity);
        Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        velocityChange = transform.TransformDirection(velocityChange);

        r.AddForce(velocityChange, ForceMode.VelocityChange);

    }
}
