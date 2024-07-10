using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public MyNetworkManager _nm;
    
    public static Vector3 StringToVector3(string sVector)
	{
		// Remove the parentheses
		if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
			sVector = sVector.Substring(1, sVector.Length-2);
		}

		// split the items
		string[] sArray = sVector.Split(',');

		// store as a Vector3
		Vector3 result = new Vector3(
			float.Parse(sArray[0]),
			float.Parse(sArray[1]),
			float.Parse(sArray[2]));

		return result;
	}
    void Start()
    {
        _nm.OnMessageReceived += Move;
    }

    void Move(object sender, MyNetworkManager.OnMessageReceivedEventArgs msg)
    {
        if (msg.opCode == "pos")
        {

            Vector3 pos = StringToVector3(msg.message);
            transform.position = pos;
        }
        if (msg.opCode == "rot")
        {

            Vector3 rot = StringToVector3(msg.message); 
            transform.eulerAngles = rot;
        }
    }
}
