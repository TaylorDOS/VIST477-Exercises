using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputData : MonoBehaviour
{
    public InputDevice rightController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rightController.isValid){
            InitializateInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        }
    }

    private void InitializateInputDevice(InputDeviceCharacteristics characteristics, ref InputDevice inputDevice)
    {
        //Debug.Log("Found controller");
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics,devices);
        if(devices.Count>0){
            inputDevice = devices[0];
        }
    }
}
