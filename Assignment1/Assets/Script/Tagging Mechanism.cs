using System;
using UnityEngine;

public class TaggingMechanism : MonoBehaviour
{

    public bool isTaggedRed = false;

    public Material blue;
    public Material red;

    public Boolean isBlue;
    public GameObject currentPlayer;

    private Renderer objectRenderer;
    private Renderer currentPlayerRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (currentPlayer)
        {
            currentPlayerRenderer = currentPlayer.GetComponent<Renderer>();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Cylinder"))
        {
            if (isBlue == true)
            {
                objectRenderer.material = blue;
                if (currentPlayer)
                {
                    currentPlayerRenderer.material = blue;
                }
                isBlue = false;
            }
            else
            {
                objectRenderer.material = red;
                if (currentPlayer)
                {
                    currentPlayerRenderer.material = red;
                }
                isBlue = true;
            }
            Debug.Log("Collision Occur!");
        }
    }
}
