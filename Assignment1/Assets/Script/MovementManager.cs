using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
 
public class MovementManager : MonoBehaviour
{
    private PhotonView myView;
    private GameObject myBody;
    private Rigidbody myRB;

    

    private float xInput;
    private float zInput;
    private float movementSpeed = 10.0f;

    private MeshRenderer myMesh;
    private int index = 0;
    [SerializeField] List<Material> myMaterials;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();
        myBody = transform.GetChild(0).gameObject;
        myRB = myBody.GetComponent<Rigidbody>();
        myMesh = myBody.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myView.RPC("changeMaterial", RpcTarget.Others);
        }
    }

    private void FixedUpdate()
    {
        myRB.AddForce(xInput * movementSpeed, 0, zInput * movementSpeed);
    }

    [PunRPC]
    void changeMaterial()
    {
        if (myView.IsMine){
            if (index == myMaterials.Count)
            {
                index = 0;
            }
            myMesh.material = myMaterials[index];
            index ++;
        }
        
    }
}
