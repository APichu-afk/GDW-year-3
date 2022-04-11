using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyCheck : MonoBehaviour
{
    public bool gamestart = false;//checks if the game has started

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gamestart = true;
        }
    }
    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gamestart = true;
        }
    }
}
