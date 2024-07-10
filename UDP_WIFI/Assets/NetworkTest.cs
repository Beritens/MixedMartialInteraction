using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkTest : MonoBehaviour
{
    public MyNetworkManager _networkManager;

    public RectTransform button;
    // Start is called before the first frame update
    void Start()
    {

        _networkManager.OnMessageReceived += HandleMessage;
    }

    void HandleMessage(object sender, MyNetworkManager.OnMessageReceivedEventArgs args)
    {
        switch (args.opCode)
        {
            case "button":
                HandleButton(args.message);
                break;
        }
    }

    void HandleButton(string msg)
    {
        Debug.Log(msg);
        button.localScale *= 2;
    }

    public void OnPress()
    {
        _networkManager.Send("button", "test");
    }

}
