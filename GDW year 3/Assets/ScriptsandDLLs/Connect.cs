using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Connect : MonoBehaviour
{
    public InputField Name;
    public InputField IPAddress;
    public static string Namestring;
    public static string IPAddressstring;
    public void Connection()
    {
        Namestring = Name.text;
        IPAddressstring = IPAddress.text;
        SceneManager.LoadScene("Online");
    }
}
