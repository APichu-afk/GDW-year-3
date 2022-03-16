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
    public InputField MessageSending;
    private string tempstatus;

    public static void RunClient()
    {
        IPAddress ip = IPAddress.Parse(Connect.IPAddressstring);//192.168.2.144");//127.0.0.1
        remoteEP = new IPEndPoint(ip, 11111);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client_socket.Connect(remoteEP);
    }

    public void chat()
    {
        byte[] message = Encoding.ASCII.GetBytes("Chatting!" + MessageSending.text);
        client_socket.Send(message);
        byte[] buffer = new byte[512];
        int recv = client_socket.Receive(buffer);
        MessageReciving.text += Encoding.ASCII.GetString(buffer, 0, recv) + "\n";
    }

    public void close()
    {
        byte[] disconnent = Encoding.ASCII.GetBytes(Name.text + " " + status.text + "D");
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
        if (status.text != tempstatus)
        {
            byte[] msg = Encoding.ASCII.GetBytes(Name.text + " " + status.text);
            client_socket.Send(msg);
            tempstatus = status.text;
        }
        
        
    }

    public void refresh()
    {
        byte[] refresh = Encoding.ASCII.GetBytes("refresh");
        client_socket.Send(refresh);
        byte[] buffer = new byte[512];
        int rec = client_socket.Receive(buffer);
        Users.text = Encoding.ASCII.GetString(buffer, 0, rec);
    }
}
