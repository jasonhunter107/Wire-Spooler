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
            //Instantiate and declare reader and writer
            BinaryWriter writer = new BinaryWriter(new BufferedStream(client.GetStream()));
            BinaryReader reader = new BinaryReader(new BufferedStream(client.GetStream())); 

           // NetworkStream writer = client.GetStream();

            //Bytes that are going to be sent to PLC
            byte[] dataArray = new byte[1000];

            //Send 5 bytes (could change, using these values for now)
            // [1 byte] command code
            // [2 bytes] payload length
            // [2 bytes] payload (inches to cut)
            writer.Write(dataArray[0]);
            writer.Write((short) 2);
            writer.Write((short)inchesToCut);
            writer.Flush();

            //wait for response from PLC
            /*
             * 
             * */

            var command = reader.ReadByte();
            var len = reader.ReadInt16();

            return (command == 0x00);
        }


    }
}