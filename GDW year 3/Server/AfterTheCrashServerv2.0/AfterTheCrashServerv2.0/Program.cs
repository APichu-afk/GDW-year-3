using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

public class Server
{
    private static byte[] buffer = new byte[512];
    private static Socket server;

    private static List<Socket> clientSockets = new List<Socket>();
    private static List<String> names = new List<String>();
    private static byte[] outBuffer = new byte[512];
    private static string outMsg = "";
    private static string msg = "";
    private static string name = "";

    static void Main(string[] args)
    {
        Console.WriteLine("Starting Server...");
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        server.Bind(new IPEndPoint(ip, 11111));
        Console.WriteLine("Server Ip: " + server.LocalEndPoint.ToString());
        server.Listen(4);

        server.BeginAccept(new AsyncCallback(AcceptCallback), null);

        /*Thread sendThread = new Thread(new ThreadStart(SendLoop));
        sendThread.Name = "SendThread";
        sendThread.Start();
        */
        Console.ReadLine();
    }

    private static void AcceptCallback(IAsyncResult result)
    {
        Socket socket = server.EndAccept(result);
        Console.WriteLine("Client Connected with IP: " + socket.RemoteEndPoint.ToString());

        clientSockets.Add(socket);

        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);

        server.BeginAccept(new AsyncCallback(AcceptCallback), null);

    }

    private static void ReceiveCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);
        byte[] data = new byte[rec];
        Array.Copy(buffer, data, rec);


        msg = Encoding.ASCII.GetString(data);
        Console.WriteLine("Received" + msg);

        //Get the players names
        int index = msg.IndexOf(":");
        if(index >= 0)
        {
            name = msg.Substring(0, index);
        }

        //Does not add name if the name is already there
        if (!names.Contains(name))
        {
            names.Add(name);
        }

        if (msg.Contains(":s:"))
        {
            //Send chatting messages
            outMsg = msg;//Eg Bob: Hello

            outBuffer = Encoding.ASCII.GetBytes(outMsg);

            //Sends the msg to each client connected
            foreach (var clients in clientSockets)
            {
                Console.WriteLine("Sending data to: " + clients.RemoteEndPoint.ToString());
                clients.BeginSend(outBuffer, 0, outBuffer.Length, 0, new AsyncCallback(SendCallback), clients);
            }
            outMsg = "";
        }

        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
    }

    private static void SendCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        socket.EndSend(result);
    }
}
