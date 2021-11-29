using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
