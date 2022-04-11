using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Lecture 4
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
public class Client : MonoBehaviour
{
    //private static byte[] outBuffer = new byte[512];
    private static IPEndPoint remoteEP;
    private static Socket client_socket;
    public Text status;
    public Text Name;
    public Text Users;
    public Text MessageReciving;
    public Text peopleready;
    public InputField MessageSending;
    public GameObject readybutton;

    public static void RunClient()
    {
        IPAddress ip = IPAddress.Parse(Connect.IPAddressstring);//192.168.2.144");//127.0.0.1
        remoteEP = new IPEndPoint(ip, 11111);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client_socket.Connect(remoteEP);
        client_socket.Blocking = false;
    }

    public void chat()
    {
        byte[] message = Encoding.ASCII.GetBytes(Name.text + ":m: " + MessageSending.text);
        client_socket.Send(message);
    }

    public void ready()
    {
        byte[] ready = Encoding.ASCII.GetBytes(":r:");
        client_socket.Send(ready);
        readybutton.SetActive(false);
    }

    public void close()
    {
        byte[] disconnent = Encoding.ASCII.GetBytes(Name.text + " D");
        client_socket.Send(disconnent);
        client_socket.Shutdown(SocketShutdown.Both);
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        RunClient();
    }

    // Update is called once per frame
    void Update()
    {

        byte[] buffer = new byte[512];
        int recv = client_socket.Receive(buffer);
        if (Encoding.ASCII.GetString(buffer, 0, recv).Contains(":m:"))
        {
            MessageReciving.text += Encoding.ASCII.GetString(buffer, 0, recv) + "\n";
        }
        if (Encoding.ASCII.GetString(buffer, 0, recv).Contains("amount of people ready:"))
        {
            peopleready.text = Encoding.ASCII.GetString(buffer, 0, recv);
        }
        if (peopleready.text == "amount of people ready: 4")
        {
            SceneManager.LoadScene("OnlineGame");
        }

    }
}