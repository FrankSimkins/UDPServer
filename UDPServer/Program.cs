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
        static void Main(string[] args)
        {
            //
            //Declare variables
            //
            List<User> userList = new List<User>();
            UdpClient udpServer = new UdpClient(8080);
            List<string> userString = new List<string>();
            


            Console.WriteLine("Running...");
            while (true)
            {
                //Check to see if we have any new connections and receive any bytes.
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    //Receive bytes, translate them to a string. And find the username in the string.
                    byte[] receivedBytes = udpServer.Receive(ref remoteEndPoint);
                    string returnData = Encoding.ASCII.GetString(receivedBytes);
                    string userName = returnData.Split(':')[0];

                    //Here we check to see if the user we just received bytes from, has already connected or not.
                    if (!userString.Contains(userName))
                    {
                        //If this is a new user, then we want to create a new one and add them to the user list.
                        User newUser = new User();
                        userString.Add(userName);
                        newUser.UserName = userName;
                        newUser.Port = remoteEndPoint.Port;
                        //newUser.Port = 8085;
                        newUser.UserIP = remoteEndPoint.Address;
                        newUser.UserEndPoint = new IPEndPoint(remoteEndPoint.Address, remoteEndPoint.Port);
                        userList.Add(newUser);
                        Console.WriteLine(userName.ToString() + ": Connected");
                    }
                    else
                    {
                        //If they are not a new user, then we want to send the message out to all users currently connected.
                        byte[] byteMessage = Encoding.ASCII.GetBytes(returnData);

                        //For each user in our list we want to try to send the message.
                        foreach (User user in userList)
                        {
                            try
                            {
                                udpServer.Send(byteMessage, byteMessage.Length, user.UserEndPoint);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                            }
                        }

                        Console.WriteLine(returnData);
                    }



                }
                catch (Exception exception)
                {
                    
                }

            }

        }
    }
}
