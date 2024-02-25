using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlanetGravity : MonoBehaviour
{
    private Transform planet;
    public bool alignToPlanet = true;

    float gravityConstant = 9.8f;
    Rigidbody r;


    void Start()
    {
        r = GetComponent<Rigidbody>();
        GameObject planetObject = GameObject.FindWithTag("Planet");
        if (planetObject != null)
        {
            // Get the transform component of the planet object
            planet = planetObject.transform;
        }
        else
        {
            Debug.LogError("Planet object not found with tag 'planet'");
        }
    
    }

    void FixedUpdate()
    {
        Vector3 toCenter = planet.position - transform.position;
        toCenter.Normalize();


        r.AddForce(toCenter * gravityConstant, ForceMode.Acceleration);

        if (alignToPlanet)
        {
            Quaternion q = Quaternion.FromToRotation(transform.up, -toCenter);
            q = q * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
        }
    }
}