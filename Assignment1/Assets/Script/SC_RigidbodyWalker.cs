using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]

public class SC_RigidbodyWalker : MonoBehaviour
{
    private PhotonView myView;
    public float speed = 30.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    Rigidbody r;
    Vector2 rotation = Vector2.zero;
    float maxVelocityChange = 10.0f;

    private float xInput;
    private float yInput;

    void Awake()
    {
        myView = GetComponent<PhotonView>();
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;
        r.useGravity = false;
        r.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rotation.y = transform.eulerAngles.y;
        if (myView.IsMine)
        {
            Transform childTransform = transform.GetChild(0);
            if (childTransform != null)
            {
                GameObject child = childTransform.gameObject;
                // Activate the child object
                child.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No child object found!");
            }
        }

    }

    void Update()
    {
        if(myView.IsMine)
        {
            yInput = Input.GetAxis("Vertical") + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
            xInput = Input.GetAxis("Horizontal")+ OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        }
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
