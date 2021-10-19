using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform theDest;

    void OnMouseDown()
    {
        this.transform.position = theDest.position;
        this.transform.parent = GameObject.Find("Destination").transform;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    void OnMouseUp()
    {
        GetComponent<Rigidbody>().freezeRotation = false;
        GetComponent<Rigidbody>().useGravity = true;
        this.transform.parent = null;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (PickUpBool) 
        {
            PickUpBool = false;
            Debug.Log("Triggered.");
            this.transform.position = theDest.position;
            this.transform.parent = GameObject.Find("Destination").transform;
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    void Update()
    {
        //Detect when the F arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.F))
        {
            PickUpBool = true;
            Debug.Log("F key was pressed.");
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().useGravity = true;
            this.transform.parent = null;
        }
    }*/

    }
