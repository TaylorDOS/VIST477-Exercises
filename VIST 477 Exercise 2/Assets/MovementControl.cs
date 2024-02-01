using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MovementControl : MonoBehaviour
{
    public Camera SceneCam;
    public TMP_Text textMeshPro;
    public float normalMoveSpeed = 3f;
    public float sprintMoveSpeed = 6f;
    public float jumpForce = 5f;
    private float currentMoveSpeed;

    private bool isGrounded;

    private Rigidbody myRigidbody; 

    private int score = 0;

    void Start()
    {
        currentMoveSpeed = normalMoveSpeed;
        myRigidbody = GetComponent<Rigidbody>();
        textMeshPro.text = "Score: " + score;
    }
    void Update()
    {
        // Check if the object is on the floor
        bool isGrounded = IsGrounded();

        // Diagonal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMoveSpeed = sprintMoveSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMoveSpeed = normalMoveSpeed;
        }
        

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        movement *= currentMoveSpeed;

        // Set the velocity based on input
        myRigidbody.velocity = new Vector3(movement.x, myRigidbody.velocity.y, movement.z);

        // Jump only when on the floor
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Apply an upward force
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpForce, myRigidbody.velocity.z);
        }

        if (transform.position.y < -5f)  // Adjust the threshold as needed
        {
            ResetPosition();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Coin" tag
        if (collision.gameObject.CompareTag("Coin"))
        {
            // Player has collided with a coin
            UpdateScoreText();

        }
        else if (collision.gameObject.CompareTag("Danger"))
        {
            ResetGame();
        }

    }

    private void UpdateScoreText()
    {
        score += 10;
        if (textMeshPro != null)
        {
            textMeshPro.text = "Score: " + score;
        }
    }

    bool IsGrounded()
    {
        // Check if the object's vertical velocity is close to zero
        return Mathf.Abs(myRigidbody.velocity.y) < 0.01f;
    }

    void ResetPosition()
    {
        // Set the object's position to a predefined reset position
        transform.position = new Vector3(0f, 5f, 0f);  // Adjust the values as needed
        myRigidbody.velocity = Vector3.zero;  // Reset velocity
    }

    void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}