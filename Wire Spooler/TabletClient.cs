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
using System.Threading;
using System.Threading.Tasks;

namespace Wire_Spooler
{
    public class TabletClient
    {
        TcpClient client;

        public TabletClient(string hostname, int port)
        {
            client = new TcpClient(hostname, port);
        }

        public event EventHandler<int> wireLengthReceived;


        public bool SpoolWire(int spoolSize, int quantity, int gauge, int lengthWire)
        {
            //Instantiate and declare network stream
            NetworkStream stream = client.GetStream();

            //String that is going to be sent to PLC
            string stringData = string.Format("01 {0} {1} {2} {3}",spoolSize, quantity, gauge, lengthWire);
            
            //Array that holds the network stream buffer
            byte[] recievedData = new byte[1024];

            //Bytes that are going to be sent to PLC
            byte[] bytes = Encoding.ASCII.GetBytes(stringData);

            //Length of buffer
            var length = 0;

            //Send the bytes to the PLC
            stream.Write(bytes, 0, bytes.Length);

            stream.Flush();

            //wait for response from PLC
            /*
             * 
             * */
            //Use string builder if appending strings

            do
            {
                length = stream.Read(recievedData, 0, recievedData.Length);
            } while (stream.DataAvailable);



            //var length = stream.Read(recievedData, 0, recievedData.Length);

            //Convert the data that was received from the PLC to a string
            var receivedString = Encoding.ASCII.GetString(recievedData, 0, length);


            return (receivedString == "Done");
        }

        public bool RunMotor(int speed)
        {
            //Instantiate and declare network stream
            NetworkStream stream = client.GetStream();

            //String that is going to be sent to PLC
            string stringData = string.Format("02 {0}", speed);

            //Array that holds the network stream buffer
            byte[] recievedData = new byte[1024];

            //Bytes that are going to be sent to PLC
            byte[] bytes = Encoding.ASCII.GetBytes(stringData);

            //Send the bytes to the PLC
            stream.Write(bytes, 0, bytes.Length);

            stream.Flush();

            //wait for response from PLC
            /*
             * 
             * */

            var length = stream.Read(recievedData, 0, recievedData.Length);


            //Convert the data that was received from the PLC to a string
            var receivedString = Encoding.ASCII.GetString(recievedData, 0, length);

            Console.WriteLine(receivedString);

            return (receivedString == "02 Ready");
        }

        public bool SendCommand(int code)
        {
            //Instantiate and declare network stream
            NetworkStream stream = client.GetStream();

            //String that is going to be sent to PLC
            string stringData;

            //Switch statement for the specific command to send
            //The commands does not need another parameter to be sent like SpoolWire and RunMotor functions
            switch (code)
            {
                case 3:
                 stringData = string.Format("03");
                 break;

                case 4:
                    stringData = string.Format("04");
                    break;

                case 5:
                    stringData = string.Format("05");
                    break;

                case 6:
                    stringData = string.Format("06");
                    break;

                case 7:
                    stringData = string.Format("07");
                    break;

                case 8:
                    stringData = string.Format("08");
                    break;

                case 9:
                    stringData = string.Format("09");
                    break;

                case 10:
                    stringData = string.Format("10");
                    break;

                case 11:
                    stringData = string.Format("11");
                    break;

                case 12:
                    stringData = string.Format("12");
                    break;

                default:
                    stringData = "00";
                    break;


            }

            //Array that holds the network stream buffer
            byte[] recievedData = new byte[1024];

            //Bytes that are going to be sent to PLC
            byte[] bytes = Encoding.ASCII.GetBytes(stringData);

            //Send the bytes to the PLC
            stream.Write(bytes, 0, bytes.Length);

            stream.Flush();

            //wait for response from PLC
            /*
             * 
             * */

            var length = stream.Read(recievedData, 0, recievedData.Length);


            //Convert the data that was received from the PLC to a string
            var receivedString = Encoding.ASCII.GetString(recievedData, 0, length);

            Console.WriteLine(receivedString);

            return (receivedString == "Done");
        }


        public async Task ReadLengthAsync (CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];
            await client.ConnectAsync("10.0.2.2", 8081);

            var stream = client.GetStream();

            while(!cancellationToken.IsCancellationRequested)
            {
                var len = await stream.ReadAsync(buffer, 0, buffer.Length);

                if(buffer[0] == 0x30)
                {
                    var wireLength = buffer[1];

                    wireLengthReceived?.Invoke(this, wireLength);
                }
            }
        }


    }
}