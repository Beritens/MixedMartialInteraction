using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConnectUI : MonoBehaviour
{
    // Start is called before the first frame update
    public MyNetworkManager networkManager;
    public TMP_InputField textField;
    public void Join()
    {
        networkManager.Join(textField.text);
        
    }
}
