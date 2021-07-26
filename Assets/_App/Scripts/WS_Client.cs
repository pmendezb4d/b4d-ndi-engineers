using UnityEngine;
using WebSocketSharp;
using System.Collections;
using System;
using TMPro;
public class WS_Client : MonoBehaviour
{
    public string endpoint;
    WebSocket ws;
    public bool isConnected;
    public float rate = 1;
    private float nextCheck;

    public static WS_Client Instance { get; private set; }

    public delegate void DataEventHandeler(string message);
    public event DataEventHandeler GetData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        ws = new WebSocket("ws://" + endpoint);
        ws.OnOpen += Connect;
        ws.OnClose += Disconect;
        ws.OnMessage += GetMessage;
    }

    private void OnDisable()
    {
        ws.OnOpen -= Connect;
        ws.OnClose -= Disconect;
        ws.OnMessage -= GetMessage;
        ws.Close();
    }

    private void Update()
    {
        if (ws != null)
        {
            if (ws.IsAlive == false && Time.time > nextCheck)
            {
                Debug.Log("try to connect");
                ws.Connect();
                nextCheck = rate + Time.time;
            }
        }
    }

    private void GetMessage(object sender, MessageEventArgs e)
    {
        if (GetData != null)
        {
            GetData(e.Data);
        }
    }
    private void Connect(object sender, System.EventArgs e)
    {
        ws.Connect();
        Debug.Log("Connection succesful");
        isConnected = true;
    }
    private void Disconect(object sender, CloseEventArgs e)
    {
        if (isConnected == true)
        {
            Debug.Log("Disconnected");
            isConnected = false;
        }
    }
}