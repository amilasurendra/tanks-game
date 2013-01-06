﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using Tanks.AI;
using System.Configuration;

namespace Tanks
{
    public class Connection
    {

        public string server {  get;  set; }
        public int serverPort { get; set; }
        public int clientPort { get; set; }

        private TcpClient clientSocket;
        private TcpListener listner;
        private NetworkStream sendStream;
        private BinaryWriter writer;


                
        public void init(){
            string clientIP = ConfigurationSettings.AppSettings["myIP"];
            listner = new TcpListener(IPAddress.Parse(clientIP), clientPort);
            listner.Start();
        }
        

        public void sendData(String data) {

            if (Program.state.tooQuickReturned) {
                Thread.Sleep(1000);
                Program.state.tooQuickReturned = false;
            }

            clientSocket = new TcpClient();
            clientSocket.Connect(server, serverPort);

            sendStream = clientSocket.GetStream();
            byte[] tmp = Encoding.ASCII.GetBytes(data);
            writer = new BinaryWriter( sendStream );
            writer.Write(tmp);
            writer.Flush();
            writer.Close();
        }

      
        public void receiveData() {

            NetworkStream stream;
            StreamReader reader;

            while (true)
            {
                try
                {

                    Socket receive = listner.AcceptSocket();
                    stream = new NetworkStream(receive);
                    reader = new StreamReader(stream);

                    if (receive.Connected)
                    {
                        string str = reader.ReadToEnd();
                        //Trace.WriteLine(str);

                        Program.state.recieved = str;

                        ThreadPool.SetMaxThreads(5,5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Program.state.update));
                        
                    }

                }
                catch (Exception ex) { Trace.WriteLine("Error: " + ex.Message); }
            }
         
        
        }
   
    }

    
}
