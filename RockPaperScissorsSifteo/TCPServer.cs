﻿using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Sifteo;

namespace RockPaperScissors {

    public interface ServerDelegate {
        void clientConnected();
        void gameOver(int winner);
    }

    public class Server {

		private bool fakeIt = false;

        private TcpListener tcpListener;
        private Thread listenThread;
        private TcpClient mClient;
        private ServerDelegate mDelegate;

        public Server(ServerDelegate theDelegate) {
            this.mDelegate = theDelegate;
            this.tcpListener = new TcpListener(IPAddress.Any, 2013);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients() {
			if (fakeIt) {
				Thread.Sleep (1000);
				this.mDelegate.clientConnected ();
				return;
			}

            this.tcpListener.Start();

            //blocks until a client has connected to the server
            mClient = this.tcpListener.AcceptTcpClient();
            this.mDelegate.clientConnected();
            Log.Debug("Client connected");

            //create a thread to handle communication 
            //with connected client
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(mClient);
        }

        public void SendMessage(String message) {
			if (fakeIt) {
				return;
			}
            Log.Debug("Sending message to client: " + message); 
            NetworkStream clientStream = mClient.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(message + "\n");

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private void HandleClientComm(object client) {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true) {
                bytesRead = 0;

                try {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                } catch {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0) {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                int winner = Convert.ToInt32(encoder.GetString(message, 0, bytesRead));
                Log.Debug(winner == 0 ? "RED wins!" : "Blue wins!");
                mDelegate.gameOver(winner);
            }

            tcpClient.Close();
        }


    }
}
