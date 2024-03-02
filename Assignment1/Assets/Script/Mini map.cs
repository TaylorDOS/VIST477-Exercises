using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    //Set Camera on Canvas->Render Camera - Unity 3D

void Start()
{
  Canvas canvas = gameObject.GetComponent<Canvas>();
  canvas.renderMode = RenderMode.ScreenSpaceCamera;
  canvas.worldCamera = Camera.main;
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
