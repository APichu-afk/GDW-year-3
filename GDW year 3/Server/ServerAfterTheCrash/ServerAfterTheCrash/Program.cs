using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class UDPServer
{
    public static List<string> ClientsList = new List<string>();
    public static void StartServer()
    {
        
        byte[] buffer = new byte[512];

        IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

        IPAddress ip = IPAddress.Parse("127.0.0.1");//hostInfo.AddressList[1];//"127.0.0.1" for testing

        Console.WriteLine($"Server Name: {hostInfo.HostName} | IP: {ip}");

        IPEndPoint localEP = new IPEndPoint(ip, 11111);

        Socket server = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //Bind, receive data

        try
        {
            server.Bind(localEP);
            server.Listen(10);

            Console.WriteLine("Waiting for data...");

            Socket handler = server.Accept();

            IPEndPoint clientEP = (IPEndPoint)handler.RemoteEndPoint;

            while (true)
            {
                int rec = handler.Receive(buffer);

                Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, rec));

                if (Encoding.ASCII.GetString(buffer, 0, rec) == "refresh")
                {
                    for (int i = 0; i < ClientsList.Count; i++)
                    {
                        byte[] users = Encoding.ASCII.GetBytes(ClientsList[i]);
                        handler.Send(users);
                    }
                }
                if(Encoding.ASCII.GetString(buffer,0,rec).StartsWith("Chatting!"))
                {
                    string tmpMessage = Encoding.ASCII.GetString(buffer, 0, rec).Remove(0, 9);
                    byte[] message = Encoding.ASCII.GetBytes(tmpMessage);
                    handler.Send(message);
                }
                else
                {
                    ClientsList.Add(Encoding.ASCII.GetString(buffer, 0, rec));
                }
                for (int i = 0; i < ClientsList.Count; i++)
                {
                    if (ClientsList[i] + "D" == Encoding.ASCII.GetString(buffer, 0, rec))
                    {
                        char trimchar = 'D';
                        string temp = Encoding.ASCII.GetString(buffer, 0, rec);
                        ClientsList.Remove(temp);
                        ClientsList.Remove(temp.TrimEnd(trimchar));
                    }
                }

                ClientsList.ForEach(Console.WriteLine);
                
                //Console.WriteLine("Received from: {0}", remoteClient.ToString());
                //Console.WriteLine("Data: {0}", Encoding.ASCII.GetString(buffer, 0, rec));

                //If client sends floats
                //Console.WriteLine("Data: {0}", BitConverter.ToSingle(buffer, 0));
            }

            //server.Shutdown(SocketShutdown.Both);
            //server.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }
}