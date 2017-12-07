using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
    class Program
    {
        static int lastMessage = 0;
        static int lastMSG;
        static string[] messageArray = new string[100024];
        static List<string> userString = new List<string>();
        static List<User> userList = new List<User>();
        

        static void Main(string[] args)
        {
            //
            //Declare variables
            //
            UdpClient udpServer = new UdpClient(8080);
            
            bool running = true;
            string command = "";
            string msg = "";
            string userName = "";
            string lMessage = "";
            


            Console.WriteLine("Running...");
            while (running)
            {

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    //Receive bytes, translate them to a string. And find the username in the string.
                    byte[] receivedBytes = udpServer.Receive(ref remoteEndPoint);
                    string returnData = Encoding.ASCII.GetString(receivedBytes);
                    command = returnData.Split(':')[0];
                    lMessage = returnData.Split(':')[1];
                    int.TryParse(lMessage, out lastMSG);
                    userName = returnData.Split(':')[2];
                    msg = returnData.Split(':')[3];
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }



                    switch (command)
                {
                    case "send":
                        SendMessage(remoteEndPoint, udpServer);
                        break;
                    case "receive":
                        ReceiveMessage(msg, userName);
                        break;

                    case "connect":
                        Connect(userName, remoteEndPoint, udpServer);
                        break;

                    case "disconnect":
                        Disconnect();
                        break;


                    default:
                        break;
                }




            }

        }

        static void SendMessage(IPEndPoint remoteEndPoint, UdpClient udpServer)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (lastMSG < lastMessage)
            {
                string returnData = lastMessage + ": " + messageArray[lastMessage];
                try
                {
                    byte[] byteMessage = Encoding.ASCII.GetBytes(returnData);
                    udpServer.Send(byteMessage, byteMessage.Length, remoteEndPoint);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else
            {
                byte[] bMessage = Encoding.ASCII.GetBytes($"{lastMessage}:");
                udpServer.Send(bMessage, bMessage.Length, remoteEndPoint);

            }

            
        }

        static void ReceiveMessage(string returnData, string userName)
        {
            //if (lastMessage > 62)
            //{
            //    lastMessage = 0;
            //}
            lastMessage++;
            messageArray[lastMessage] = userName + ": " + returnData;
            Console.WriteLine($"{userName}: {returnData} : {lastMessage}");
        }

        static void Connect(string userName, IPEndPoint remoteEndPoint, UdpClient udpServer)
        {
            User newUser = new User();
            userString.Add(userName);
            newUser.UserName = userName;
            newUser.Port = remoteEndPoint.Port;
            //newUser.Port = 8085;
            newUser.UserIP = remoteEndPoint.Address;
            newUser.UserEndPoint = new IPEndPoint(remoteEndPoint.Address, remoteEndPoint.Port);
            userList.Add(newUser);
            Console.WriteLine(userName.ToString() + ": Connected");

            string returnData = lastMessage.ToString() + ":";
            byte[] byteMessage = Encoding.ASCII.GetBytes(returnData);

            try
            {
                udpServer.Send(byteMessage, byteMessage.Length, remoteEndPoint);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        static void Disconnect()
        {
        }
    }
}
