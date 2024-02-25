using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravityBody))]
public class FirstPersonController : MonoBehaviour
{
    // Public variables
    public float mouseSensitivityX = 2;
    public float mouseSensitivityY = 2;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    public LayerMask groundedMask;

    // System variables
    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    Rigidbody rigidbody;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Look rotation:
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        // Calculate movement:
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rigidbody.AddForce(transform.up * jumpForce);
            }
        }

        // Grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5 + .1f, groundedMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    // private void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Prop"))
    //     {
    //         Debug.Log("Player collided with prop object.");

    //         // Reset movement variables
    //         moveAmount = Vector3.zero;
    //         smoothMoveVelocity = Vector3.zero;

    //         // Stop rigidbody movement
    //         rigidbody.velocity = Vector3.zero;

    //         // Calculate new position to move player outside of the prop object
    //         Vector3 newPosition = transform.position;
    //         foreach (ContactPoint contact in collision.contacts)
    //         {
    //             Vector3 normalDirection = contact.normal.normalized;
    //             newPosition += normalDirection * 0.1f; // Move player slightly away from collision point
    //         }

    //         // Move player to the new position
    //         transform.position = newPosition;
    //     }
    // }

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
}