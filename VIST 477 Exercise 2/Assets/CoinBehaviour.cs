using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    private Vector3 myStartPosition;
    private AudioSource myAudio;
    private MeshRenderer myRenderer;
    private SphereCollider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myStartPosition = transform.position;
        myAudio = GetComponent<AudioSource>();
        myRenderer = GetComponent<MeshRenderer>();
        myCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myStartPosition + new Vector3(0.0f, Mathf.Sin(Time.time) * 0.5f ,0.0f);
        transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);

    }

    private void OnCollisionEnter(Collision collision)
    {
        myAudio.Play();
        myRenderer.enabled = false;
        myCollider.enabled = false;
    }

}
