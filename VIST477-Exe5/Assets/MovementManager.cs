using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class MovementManager : MonoBehaviour
{
    private PhotonView myView;
    private float xInput;
    private float yInput;
    private float movementSpeed = 10.0f;
    private InputData inputData;
    //[SerializeField] private GameObject myObjectToMove; 
    private Transform myXRRig;

    private Rigidbody myRB;

    private GameObject myChild;
    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();
        
        myChild = transform.GetChild(0).gameObject;
        myRB = myChild.GetComponent<Rigidbody>();

        GameObject myXrOrigin = GameObject.Find("XR Origin (XR Rig)");
        myXRRig = myXrOrigin.transform;
        inputData = myXrOrigin.GetComponent<InputData>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            myXRRig.position = myChild.transform.position;
            if(inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
            {
                xInput = movement.x;
                yInput = movement.y;
            }
        }   
    }

    void FixedUpdate()
    {
        myRB.AddForce(xInput * movementSpeed, 0, yInput * movementSpeed);
    }
}
