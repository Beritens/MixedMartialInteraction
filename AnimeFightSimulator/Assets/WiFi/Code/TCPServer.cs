using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace LocalNetworking
{
    public class TCPServer : MonoBehaviour {  	
	#region private members 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener; 
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient; 	
	#endregion 	
	public Action<NetworkManager.Message> OnData;
		
	// Use this for initialization
	public void Host () { 		
		// Start TcpServer background thread 		
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 	
	}  	
	
	// Update is called once per frame
	public void Send (string opCode, string message) { 		
		SendMessage(opCode, message);         
	}  	
	
	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Any, 8052); 			
			tcpListener.Start();              
			IPEndPoint tempEndpoint = (IPEndPoint)tcpListener.LocalEndpoint;
            IPAddress localIpAddress = tempEndpoint.Address;

            // Print out the local IP address
            Debug.LogError($"Listening on IP address: {localIpAddress}");
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[2048];  			
			while (true) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string messageJson = Encoding.ASCII.GetString(incommingData);
							Debug.LogError("gor this: " +  messageJson);
							string unpacked = Util.UnpackJson(messageJson);
							if (unpacked == null) break;	
							Debug.LogError(unpacked);
							NetworkManager.Message msg = JsonUtility.FromJson<NetworkManager.Message>(unpacked);
							UnityMainThreadDispatcher.Instance().Enqueue(OnDataCO(msg));						
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	
	
	private IEnumerator OnDataCO(NetworkManager.Message msg)
	{
		OnData?.Invoke(msg);
		yield return new WaitForEndOfFrame();
	}
	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	private void SendMessage(string opCode, string message) { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
				 NetworkManager.Message msg = new NetworkManager.Message(opCode, message);

				string toSend = Util.PackJson(JsonUtility.ToJson(msg).ToString());
				toSend += "/end/";
				Debug.LogError("Sending Message: " + toSend);

				byte[] bytes = Encoding.ASCII.GetBytes(toSend);		
				// Write byte array to socketConnection stream.               
				stream.Write(bytes, 0, bytes.Length);               
				Debug.Log("Server sent his message - should be received by client");           
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 
}
    
}
