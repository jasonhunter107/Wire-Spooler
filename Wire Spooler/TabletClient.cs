using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Wire_Spooler
{
    class TabletClient
    {
        TcpClient client;

        public TabletClient(string hostname, int port)
        {
            client = new TcpClient(hostname, port);
        }


        public bool CutWire(int inchesToCut)
        {
            //Instantiate and declare network stream
            NetworkStream stream = client.GetStream();

            //String that is going to be sent to PLC
            string stringData = "01 Status Request";
            
            //Array that holds the network stream buffer
            byte[] recievedData = new byte[1024];

            //Bytes that are going to be sent to PLC
            byte[] bytes = Encoding.ASCII.GetBytes(stringData);

            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    Console.WriteLine(bytes[i]);
            //}

            //Send the bytes to the PLC
            stream.Write(bytes, 0, bytes.Length);

            stream.Flush();

            //wait for response from PLC
            /*
             * 
             * */
            //Use string builder if appending strings

            var length = stream.Read(recievedData, 0, recievedData.Length);


            //Convert the data that was received from the PLC to a string
            var receivedString = Encoding.ASCII.GetString(recievedData, 0, length);

            Console.WriteLine(receivedString);

            return (receivedString == "01 Ready");
        }


    }
}