using Jangada;
using JangadaWinClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JangadaWinClient
{
    public class TCPClient
    {
        private static TCPClient instance;
        public static TCPClient getInstance()
        {
            return instance;
        }

        private const int SleepTimeout = 50;// miliseconds

        int port = 7777;
        TcpClient tcp;
        public TCPClient(int port)
        {
            this.port = port;
            instance = this;
        }

        public void StartConnect()
        {
            Connect("127.0.0.1", port, (bytes) =>
            {
                Console.Write(bytes);
                if (!Jangada.getInstance().useProto)
                {
                    NetworkMessage inMessage = new NetworkMessage(bytes);
                    int size = (int)BitConverter.ToUInt32(inMessage.Buffer, 0) + 4;
                    inMessage.Length = size;
                    inMessage.PrepareToRead();

                    while (inMessage.Position < inMessage.Length - 1)
                    {
                        byte type = inMessage.GetByte();
                        if (!Parser.Parse(type, inMessage))
                        {
                            //Disconnect
                        }
                    }

                }
                else
                {
                    Messages messages = Messages.CreateBuilder().MergeFrom(bytes).Build();
                    foreach (Networkmessage message in messages.NetworkmessageList)
                    {
                        if (!Parser.Parse(message.Type, message))
                        {
                            //Disconnect
                        }
                    }
                }
            });
        }


        private Task Connect(string ip, int port, Action<byte[]> onReceived)
        {
            tcp = new TcpClient();
            tcp.Connect(ip, port);
            return Task.Factory.StartNew(() =>
                {
                    var receivedLines = new List<string>();

                    while (tcp.Connected)
                    {
                        var available = tcp.Available;

                        if (available == 0)
                        {
                            Thread.Sleep(SleepTimeout);
                            continue;
                        }

                        var buffer = new byte[available];
                        tcp.GetStream().Read(buffer, 0, available);
                        onReceived(buffer);
                    }

                    tcp.Close();
                });
        }

        public void Send(Messages message)
        {
            message.WriteTo(tcp.GetStream());
        }

        public void Send(Network.NetworkMessage message)
        {
            message.PrepareToSend();
            tcp.GetStream().Write(message.Buffer, 0, message.Length);
        }
    }
}
