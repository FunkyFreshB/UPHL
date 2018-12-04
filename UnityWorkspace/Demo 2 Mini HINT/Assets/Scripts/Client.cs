using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {

    NetworkClient myClient;
    const short m_MessageType = MsgType.Highest + 1;
    // Use this for initialization
    void Start () {
        //SetupLocalClient();
        SetupClient();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    // Create a client and connect to the server port
    public void SetupClient() {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("192.168.43.97", 4444);
        //isAtStartup = false;
        myClient.RegisterHandler(m_MessageType, OnMessageRecieved);
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient() {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        //isAtStartup = false;
    }

    public void OnConnected(NetworkMessage nm) {
        Debug.Log("Connected to server!");
        gameObject.GetComponent<TextMesh>().text = "BitConnect!";
    }

    public void OnMessageRecieved(NetworkMessage nm) {
        Debug.Log("Message recieved");
        string message = nm.reader.ReadString();
        Debug.Log(message);
    }
}
