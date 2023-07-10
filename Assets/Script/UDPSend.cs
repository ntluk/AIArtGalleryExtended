using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class UDPSend : MonoBehaviour
{
    private static int localPort;

    public string IP;  
    public int port;  

    IPEndPoint remoteEndPoint;
    UdpClient client;

    string strMessage = "";


    private static void Main()
    {
        UDPSend sendObj = new UDPSend();
        sendObj.init();
        sendObj.sendEndless(" endless infos \n");
    }
   
    public void Start()
    {
        init();
    }

    public void init()
    {
        print("UDPSend.init()");

        IP = "172.16.1.74";
        port = 8051;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        print("Sending to " + IP + " : " + port);
        print("Testing: nc -lu " + IP + " : " + port);

    }

    private void inputFromConsole()
    {
        try
        {
            string text;
            do
            {
                text = Console.ReadLine();

                if (text != "")
                {   
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }

    }

    public void sendString(string message)
    {
        try
        {
            
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
            print("sent");
            
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);
        }
        while (true);
    }

}