using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterCamera : MonoBehaviour
{
    public RenderTexture player2;
    public RenderTexture player3;
    public RenderTexture player4;
    public Camera monstercam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputSystem.onDeviceChange +=
       (device, change) =>
       {
           switch (change)
           {
               case InputDeviceChange.Added:
                   // New Device.
                   monstercam.targetTexture = player3;
                   break;
               case InputDeviceChange.Disconnected:
                   // Device got unplugged.
                   break;
               case InputDeviceChange.Reconnected:
                   monstercam.targetTexture = player3;
                   // Plugged back in.
                   break;
               default:
                   // See InputDeviceChange reference for other event types.
                   break;
           }
       };
    }
}
