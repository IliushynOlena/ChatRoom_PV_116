﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    class ChatServer
    {
        const short port = 4040;
        const string JOIN_CMD = "$<join>";
        UdpClient server = new UdpClient(port);
        HashSet<IPEndPoint> members = new HashSet<IPEndPoint>();
        IPEndPoint clientEndPoint = null;
        private void AddMember(IPEndPoint member)
        {
            members.Add(member);
            Console.WriteLine("Member was added!!!");
        }
        private void SendToAll(byte[] data)
        {
            foreach (IPEndPoint member in members)
            {
                server.SendAsync(data, data.Length, member);
            }

        }
        public void Start()
        {
            while (true)
            {
                byte[] data = server.Receive(ref clientEndPoint);
                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine($" {message} at {DateTime.Now.ToShortTimeString()}" +
                    $" from {clientEndPoint}");
                if (JOIN_CMD == message)
                {
                    AddMember(clientEndPoint);
                }
                else
                {
                    SendToAll(data);
                }
            }
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.Start();          
        
        }
    }
}
