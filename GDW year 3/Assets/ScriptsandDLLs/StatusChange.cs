using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusChange : MonoBehaviour
{
    public Text status;
    public Text Name;

    void Awake()
    {
        Name.text = Connect.Namestring;
    }
    public void Available()
    {
        status.text = "Available";
    }

    public void Busy()
    {
        status.text = "Busy";
    }

    public void Chatting()
    {
        status.text = "Chatting";
    }
}