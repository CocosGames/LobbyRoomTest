using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using UnityEngine.UI;

public class TestLobbyRoom : MonoBehaviour
{
    private ColyseusClient CC;
    
    // Start is called before the first frame update
    async Task Start()
    {
        ColyseusClient client = new ColyseusClient("ws://localhost:2567");
        this.CC = client;
        try {
            ColyseusRoom<MyRoomState> room = await client.Create<MyRoomState>("my_lobby");
            Debug.Log("joined successfully");
            room.OnMessage<RoomsMessage>("rooms", (message) => {
                Debug.Log ("rooms message");
                Debug.Log(message);
            });
            room.OnMessage<RoomsMessage>("+", (message) => {
                Debug.Log ("room+ message");
                Debug.Log(message);
            });
            room.OnMessage<RoomsMessage>("-", (message) => {
                Debug.Log ("room- message");
                Debug.Log(message);
            });
        } catch (Exception ex) {
            Debug.Log("join error");
            Debug.Log(ex.Message);
        }
    }
    
    public void CreateRoom()
    {
        this.CC.Create("my_game");
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}


internal class RoomsMessage
{
}
