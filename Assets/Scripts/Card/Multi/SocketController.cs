using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace YWVR.Card.Multi
{

	public class SocketController : MonoBehaviour
	{
		#region private members 	
		private TcpClient socketConnection;
		private Thread clientReceiveThread;
		private GameController gameController;
		private int color = -1;
		#endregion
		// Use this for initialization 	
		void Start()
		{
			gameController = GetComponent<GameController>();
			ConnectToTcpServer();
		}
		// Update is called once per frame
		void Update()
		{
		}
		/// <summary> 	
		/// Setup socket connection. 	
		/// </summary> 	
		private void ConnectToTcpServer()
		{
			try
			{
				clientReceiveThread = new Thread(new ThreadStart(ListenForData));
				clientReceiveThread.IsBackground = true;
				clientReceiveThread.Start();
			}
			catch (Exception e)
			{
				Debug.Log("On client connect exception " + e);
			}
		}
		/// <summary> 	
		/// Runs in background clientReceiveThread; Listens for incomming data. 	
		/// </summary>     
		private void ListenForData()
		{
			try
			{
				socketConnection = new TcpClient("139.180.220.61", 33000);
				Byte[] bytes = new Byte[4096];
				while (true)
				{
					// Get a stream object for reading 				
					using (NetworkStream stream = socketConnection.GetStream())
					{
						int length;
						// Read incomming stream into byte arrary. 					
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							// Convert byte array to string message. 						
							string serverMessages = Encoding.ASCII.GetString(incommingData);
							var msgs = serverMessages.Split(new[] { "{break}" }, StringSplitOptions.None);
							foreach(var msg in msgs)
                            {
                                if (!String.IsNullOrEmpty(msg))
                                {
                                    //Debug.Log("Message received: " + msg);
                                    HandleMessage(msg);
								}
                            }
						}
					}
				}
			}
			catch (SocketException socketException)
			{
				Debug.Log("Socket exception: " + socketException);
			}
		}

		private void HandleMessage(string msg)
		{
            try
            {
                //Debug.Log(msg.Substring(0, 8));
                //Debug.Log(msg.Substring(8));
                if (msg.Substring(0, 6) == "{Game}")
                {
                    var json = msg.Substring(6);
                    var t = JsonConvert.DeserializeObject<Game>(json);
                    //Debug.Log(t);
                    gameController.SetGame(t);
                }
                else if (msg.Substring(0, 8) == "{Player}")
                {
                    var json = msg.Substring(8);
                    var t = JsonConvert.DeserializeObject<Player>(json);
                    //Debug.Log(t);
                    color = t.Color;
                    gameController.SetPlayer(t);
                }
                else if (msg.Substring(0, 9) == "{Players}")
                {
                    var json = msg.Substring(9);
                    var t = JsonConvert.DeserializeObject<List<Player>>(json);
                    //Debug.Log(t);
                    gameController.SetPlayers(t);
                }
            }
            catch (Exception)
            {
            }
		}
		/// <summary> 	
		/// Send message to server using socket connection. 	
		/// </summary> 	
		public void SendMessageToServer(string msg)
		{
			if (socketConnection == null)
			{
				return;
			}
			try
			{
				// Get a stream object for writing. 			
				NetworkStream stream = socketConnection.GetStream();
				if (stream.CanWrite)
				{
					//string clientMessage = "This is a message from one of your clients.";
					// Convert string message to byte array.                 
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(msg + "{break}");
					// Write byte array to socketConnection stream.                 
					stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    //Debug.Log("Message Sent: " + msg);
                }
			}
			catch (SocketException socketException)
			{
				Debug.Log("Socket exception: " + socketException);
			}
		}
	}

}