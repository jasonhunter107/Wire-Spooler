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

        public TabletClient()
        {
            client = new TcpClient();
        }

        public event EventHandler<int> WireLengthReceived;
        public event EventHandler<string> StatusMsgReceived;
        public event EventHandler<string> AlarmMsgReceived;
        public event EventHandler<int> ServoPosReceived;

        public Task ConnectAsync(string hostname, int port)
        {
            return client.ConnectAsync(hostname, port);
        }

        public bool SpoolWire(int spoolSize, int quantity, int gauge, int lengthWire)
        {
            //Instantiate and declare network stream
            NetworkStream stream = client.GetStream();

            //String that is going to be sent to PLC
            string stringData = string.Format("01 {0} {1} {2} {3}", spoolSize, quantity, gauge, lengthWire);

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


            return (recievedData[0] == 0x01);
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

            return (recievedData[0] == 0x01);
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

            return (recievedData[0] == 0x01);
        }


        public async Task ReceiveDataAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[1028];

            var commandCode = 0x00;
            var stream = client.GetStream();

            while (!cancellationToken.IsCancellationRequested)
            {
                var len = await stream.ReadAsync(buffer, 0, 1);
                commandCode = buffer[0];

                //Depending on command code, parse the incoming buffer
                switch (commandCode)
                {
                    case 0x01:

                        var length = await stream.ReadAsync(buffer, 0, 80);
                        var statusMsg = Encoding.ASCII.GetString(buffer);

                        //Update Status message (80 Bytes long) [0-79]
                        StatusMsgReceived?.Invoke(this, statusMsg);
                        break;

                    case 0x02:
                        //Update Alarm Message (80 Bytes long) [80-159]
                        break;
                    case 0x03:
                        //Update Feed (4 bytes long (real) ) [160-163]
                        break;
                    case 0x04:
                        //Update servo pos (4 bytes (reak) ) [164-167]
                        break;
                    default:
                        //Unknown command
                        break;

                }
            }

        }


    }
}