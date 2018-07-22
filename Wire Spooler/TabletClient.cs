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

        //Event handlers for when the tablet receives data from the PLC
        public event EventHandler<int> WireLengthReceived;
        public event EventHandler<string> StatusMsgReceived;
        public event EventHandler<string> AlarmMsgReceived;
        public event EventHandler<int> ServoPosReceived;

        public Task ConnectAsync(string hostname, int port)
        {
            return client.ConnectAsync(hostname, port);
        }

        public void Close()
        {
            client.Close();
        }

        public async Task SendSpoolWireCodeAsync(CancellationToken cancellationToken, 
            int commandCode, int spoolSize, int quantity, int gauge, int lengthWire)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[18];

            //Spool size in bytes
            byte[] spoolSizeBytes = new byte[4];
            spoolSizeBytes[0] = (byte)spoolSize;

            //Quantity in bytes
            byte[] quantityBytes = new byte[4];

            //Gauge in bytes
            byte[] gaugeBytes = new byte[4];
            gaugeBytes[0] = (byte)gauge;

            //Length of wire in bytes
            byte[] lengthWireBytes = new byte[4];
            lengthWireBytes[0] = (byte)lengthWire;

            buffer[0] = 0x01; //command code

            switch (commandCode)
            {
                case 100:
                    Buffer.BlockCopy(lengthWireBytes, 0, buffer, 1, 4); //length of wire
                    break;
                case 103:
                    Buffer.BlockCopy(spoolSizeBytes, 0, buffer, 1, 4); //spoolSize
                    break;
                case 105:
                    Buffer.BlockCopy(quantityBytes, 0, buffer, 1, 4); //quantity
                    break;
                case 106:
                    Buffer.BlockCopy(gaugeBytes, 0, buffer, 1, 4); //gauge
                    break;
                default:
                    break;
            }


            //Buffer.BlockCopy(spoolSizeBytes, 0, buffer, 1, 4); //spoolSize
            //Buffer.BlockCopy(quantityBytes, 0, buffer, 5, 4); //quantity
            //Buffer.BlockCopy(gaugeBytes, 0, buffer, 10, 4); //gauge
            //Buffer.BlockCopy(lengthWireBytes, 0, buffer, 14, 4); //length of wire

            //buffer [1-4] = spoolSize;
            //buffer [5-9] = quantity;
            //buffer [10-13] = gauge
            //buffer [14-17] = lengthWire 

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();


        }

        public async Task SendRunMotorCommandAsync(CancellationToken cancellationToken, float speed)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Array that holds the network stream buffer
            byte[] speedBytes = BitConverter.GetBytes(speed);

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[6];

            buffer[0] = 0x68; //command code - 104
            Buffer.BlockCopy(speedBytes, 0, buffer, 1, 4); //speed

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();
        }

        public async Task SendActuatorSpeedAsync(CancellationToken cancellationToken, float speed)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Array that holds the network stream buffer
            byte[] speedBytes = BitConverter.GetBytes(speed);

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[6];

            buffer[0] = 0x65; //command code - 101
            Buffer.BlockCopy(speedBytes, 0, buffer, 1, 4); //speed

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();
        }

        public async Task SendSpoolSizeAsync(CancellationToken cancellationToken, float spoolSize)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Array that holds the network stream buffer
            byte[] speedBytes = BitConverter.GetBytes(spoolSize);

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[6];

            buffer[0] = 0x67; //command code
            Buffer.BlockCopy(speedBytes, 0, buffer, 1, 4); //speed

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();
        }

        public async Task SendLengthAsync(CancellationToken cancellationToken, float length)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Array that holds the network stream buffer
            byte[] speedBytes = BitConverter.GetBytes(length);

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[6];

            buffer[0] = 0x64; //command code 100
            Buffer.BlockCopy(speedBytes, 0, buffer, 1, 4); //speed

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();
        }

        public async Task SendQuantityAsync(CancellationToken cancellationToken, float quantity)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Array that holds the network stream buffer
            byte[] speedBytes = BitConverter.GetBytes(quantity);

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[6];

            buffer[0] = 0x69; //command code 105
            Buffer.BlockCopy(speedBytes, 0, buffer, 1, 4); //speed

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();
        }

        public async Task SendGaugeCommandAsync(CancellationToken cancellationToken, float gauge)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //01 spoolSize, quantity, gauge, lengthWire

            //Array that holds the network stream buffer
            byte[] speedBytes = BitConverter.GetBytes(gauge);

            //Bytes that are going to be sent to PLC
            byte[] buffer = new byte[6];

            buffer[0] = 0x6A; //command code 106
            Buffer.BlockCopy(speedBytes, 0, buffer, 1, 4); //speed

            //Send the bytes to the PLC
            await stream.WriteAsync(buffer, 0, 5);

            await stream.FlushAsync();
        }

        public async Task SendCommandAsync(CancellationToken cancellationToken, int code)
        {
            //Instantiate and declare network stream
            var stream = client.GetStream();

            //Bytes that are going to be sent to PLC
            byte[] bytes = new byte[1024];

            //Switch statement for the specific command to send
            //The commands does not need another parameter to be sent like SpoolWire and RunMotor functions
            switch (code)
            {
                case 0:
                    bytes[0] = 0x00;
                    break;
                case 1:
                    bytes[0] = 0x01;
                    break;
                case 3:
                    bytes[0] = 0x03;
                    break;

                case 4:
                    bytes[0] = 0x04;
                    break;

                case 5:
                    bytes[0] = 0x05;
                    break;

                case 6:
                    bytes[0] = 0x06;
                    break;

                case 7:
                    bytes[0] = 0x07;
                    break;

                case 8:
                    bytes[0] = 0x08;
                    break;

                case 9:
                    bytes[0] = 0x09;
                    break;

                case 10:
                    bytes[0] = 0x0A;
                    break;

                case 11:
                    bytes[0] = 0x0B;
                    break;

                case 12:
                    bytes[0] = 0x0C;
                    break;

                default:
                    bytes[0] = 0xF0;
                    break;

            }

            //Send the bytes to the PLC
            await stream.WriteAsync(bytes, 0, 1);

            await stream.FlushAsync();


            //var length = stream.Read(recievedData, 0, recievedData.Length);


            ////Convert the data that was received from the PLC to a string
            //var receivedString = Encoding.ASCII.GetString(recievedData, 0, length);

            //Console.WriteLine(receivedString);

            //return (recievedData[0] == 0x01);
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
                        var length = await stream.ReadAsync(buffer, 0, 80); //may need to add seperate function to make sure its reading 80 bytes
                        var statusMsg = Encoding.ASCII.GetString(buffer);

                        //Update Status message (80 Bytes long) [0-79]
                        StatusMsgReceived?.Invoke(this, statusMsg);
                        break;

                    case 0x02:
                        //Update Alarm Message (80 Bytes long) [80-159]
                        var alarmLength = await stream.ReadAsync(buffer, 0, 80);
                        var alarmMsg = Encoding.ASCII.GetString(buffer);

                        //Update Status message (80 Bytes long) [0-79]
                        AlarmMsgReceived?.Invoke(this, alarmMsg);
                        break;
                    case 0x03:
                        //Update Feed (4 bytes long (real) ) [160-163]
                        var feedLength = await stream.ReadAsync(buffer, 0, 4);

                        // If the system architecture is little-endian (that is, little end first),
                        // reverse the byte array.
                        //if (BitConverter.IsLittleEndian)
                        //    Array.Reverse(bytes);

                        var feedNum = BitConverter.ToInt32(buffer, 0);
                        break;
                    case 0x04:
                        //Update servo pos (4 bytes (reak) ) [164-167]
                        var servoPos = await stream.ReadAsync(buffer, 0, 4);

                        // If the system architecture is little-endian (that is, little end first),
                        // reverse the byte array.
                        //if (BitConverter.IsLittleEndian)
                        //    Array.Reverse(bytes);

                        var servoPosNum = BitConverter.ToInt32(buffer, 0);
                        break;

                    default:
                        //Unknown command
                        break;

                }
            }

        }


    }
}