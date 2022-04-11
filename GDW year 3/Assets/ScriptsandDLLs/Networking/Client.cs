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
    private static IPEndPoint TCPremoteEP;
    private static IPEndPoint UDPremoteEP;
    private static Socket client_socket;
    private static Socket client_socketUDP;
    public Text status;
    public Text Name;
    public Text Users;
    public Text MessageReciving;
    public Text peopleready;
    public Text people;
    public Text scoreslist;
    public InputField MessageSending;
    public GameObject readybutton;
    public GameObject character;
    public Rigidbody rigidbodycharacter;
    private float[] pos;
    private byte[] bpos;
    private int randomscorerand;
    private int randomscore;

    public static void RunClient()
    {
        IPAddress ip = IPAddress.Parse(Connect.IPAddressstring);//192.168.2.144");//127.0.0.1
        TCPremoteEP = new IPEndPoint(ip, 11111);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client_socket.Connect(TCPremoteEP);
        client_socket.Blocking = false;
    }
    public static void RunUDPClient()
    {
        IPAddress ip = IPAddress.Parse(Connect.IPAddressstring);
        UDPremoteEP = new IPEndPoint(ip, 11112);
        client_socketUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    //chatting function sends messages
    public void chat()
    {
        byte[] message = Encoding.ASCII.GetBytes(Name.text + ":m: " + MessageSending.text);
        client_socket.Send(message);
    }

    //ready function sends if you are ready or not
    public void ready()
    {
        byte[] ready = Encoding.ASCII.GetBytes(":r:");
        client_socket.Send(ready);
        readybutton.SetActive(false);
    }

    //Refresh function checks who is online
    public void refresh()
    {
        byte[] refresh = Encoding.ASCII.GetBytes(":n:");
        client_socket.Send(refresh);
    }

    //Scores function can't get scores but this will send a random score to the server
    public void scores()
    {
        randomscorerand = UnityEngine.Random.Range(1, 4);
        switch (randomscorerand)
        {
            case 1:
                randomscore = 1000;
                break;
            case 2:
                randomscore = 2000;
                break;
            case 3:
                randomscore = 3000;
                break;
        }
        byte[] scores = Encoding.ASCII.GetBytes(":s:" + randomscore);
        client_socket.Send(scores);
    }

    //close function closes the clients and sends the message to the server to tell other clients
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
        //gets the character
        character = GameObject.Find("MonsterRetopology");

        //runs tcp
        RunClient();

        //runs udp
        RunUDPClient();

        pos = new float[] { character.transform.position.x, character.transform.position.y, character.transform.position.z };
        bpos = new byte[pos.Length * 4];
    }

    // Update is called once per frame
    void Update()
    {

        pos = new float[] {character.transform.position.x, character.transform.position.y, character.transform.position.z };

        byte[] buffer = new byte[512];
        int recv = client_socket.Receive(buffer);

        //If the message has :m: it means it is a message
        if (Encoding.ASCII.GetString(buffer, 0, recv).Contains(":m:"))
        {
            MessageReciving.text += Encoding.ASCII.GetString(buffer, 0, recv) + "\n";
        }

        //If the message has amount of people ready:
        if (Encoding.ASCII.GetString(buffer, 0, recv).Contains("amount of people ready:"))
        {
            peopleready.text = Encoding.ASCII.GetString(buffer, 0, recv);
        }

        //if the message has :n: it means it is sending the player names
        if (Encoding.ASCII.GetString(buffer, 0, recv).Contains(":n:"))
        {
            people.text = Encoding.ASCII.GetString(buffer, 0, recv);
        }
        
        //if the message has :s: it means it is sending the scores
        if (Encoding.ASCII.GetString(buffer, 0, recv).Contains(":s:"))
        {
            scoreslist.text = Encoding.ASCII.GetString(buffer, 0, recv);
        }

        //Starts game
        if (peopleready.text == "amount of people ready: 4")
        {
            SceneManager.LoadScene("OnlineGame");
        }

        //If the character is moving send the coordinates
        //Doesnt work on the server side for some reason
        //if (rigidbodycharacter.velocity.magnitude > 0)
        //{
            Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

            client_socketUDP.SendTo(bpos, UDPremoteEP);
            //Debug.Log("yes");
        //}
    }
}