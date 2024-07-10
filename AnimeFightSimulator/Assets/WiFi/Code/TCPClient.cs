using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace LocalNetworking
{
    public class TCPClient: MonoBehaviour
    {
        #region private members 	
        	private TcpClient socketConnection; 	
        	private Thread clientReceiveThread; 	
			public Action<NetworkManager.Message> OnData;
			private string _ip;
        	#endregion  	
        	// Use this for initialization 	
        	public void Join (string ip)
	        {
		        _ip = ip;
        		ConnectToTcpServer();     
        	}  	
        	// Update is called once per frame
        	public void Send (string opCode, string message) { 		
				SendMessage(opCode, message);         
			}  	
        	/// <summary> 	
        	/// Setup socket connection. 	
        	/// </summary> 	
        	private void ConnectToTcpServer () { 		
		        
        		try {  			
        			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
        			clientReceiveThread.IsBackground = true; 			
        			clientReceiveThread.Start();  		
        		} 		
        		catch (Exception e) { 			
        			Debug.Log("On client connect exception " + e); 		
        		} 	
        	}  	
        	/// <summary> 	
        	/// Runs in background clientReceiveThread; Listens for incomming data. 	
        	/// </summary>     
        	private void ListenForData() { 		
        		try { 			
			        Debug.Log(_ip);
        			socketConnection = new TcpClient(_ip, 8052);  			
        			Byte[] bytes = new Byte[2048];             
        			while (true) { 				
        				// Get a stream object for reading 				
        				using (NetworkStream stream = socketConnection.GetStream()) { 					
        					int length; 					
        					// Read incomming stream into byte arrary. 					
        					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
        						var incommingData = new byte[length]; 						
        						Array.Copy(bytes, 0, incommingData, 0, length); 						
        						// Convert byte array to string message. 						
								string messageJson = Encoding.ASCII.GetString(incommingData);
								string unpacked = Util.UnpackJson(messageJson);
								if (unpacked == null) break;	
								NetworkManager.Message msg = JsonUtility.FromJson<NetworkManager.Message>(unpacked);
								UnityMainThreadDispatcher.Instance().Enqueue(OnDataCO(msg));						
        					} 				
        				} 			
        			}         
        		}         
        		catch (SocketException socketException) {             
        			Debug.Log("Socket exception: " + socketException);         
        		}     
        	}  	
	        
			private IEnumerator OnDataCO(NetworkManager.Message msg)
			{
				OnData?.Invoke(msg);
				yield return new WaitForEndOfFrame();
			}
        	/// <summary> 	
        	/// Send message to server using socket connection. 	
        	/// </summary> 	
        	private void SendMessage(string opCode, string message) {         
        		if (socketConnection == null) {             
        			return;         
        		}  		
        		try { 			
        			// Get a stream object for writing. 			
        			NetworkStream stream = socketConnection.GetStream(); 			
        			if (stream.CanWrite) {                 
						 NetworkManager.Message msg = new NetworkManager.Message(opCode, message);

						string toSend = Util.PackJson(JsonUtility.ToJson(msg).ToString());
						toSend += "/end/";
						Debug.LogError("Sending Message: " + toSend);

						byte[] bytes = Encoding.ASCII.GetBytes(toSend);		
						Debug.LogError(bytes.Length);
						Debug.LogError(toSend);
						Debug.LogError(toSend.Length);
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
