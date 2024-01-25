using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Started");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Print a message to the console
            Debug.Log("Space button pressed");
        }

    }
}
