using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using UnityEngine.UI;
using Object = System.Object;
using Type = System.Type;


public class TestLobbyRoom : MonoBehaviour
{
    private ColyseusClient CC;
    
    private static void GetPropertyValues(Object obj)
    {
        Type t = obj.GetType();
        Debug.Log("Type is: "+ t.Name);
        PropertyInfo[] props = t.GetProperties();
        Debug.Log("Properties (N): "+ 
            props.Length);
        foreach (var prop in props)
            if (prop.GetIndexParameters().Length == 0)
                Debug.Log("    "+ prop.Name+" "+
                    prop.PropertyType.Name+" "+
                    prop.GetValue(obj));
            else
                Debug.Log("   "+ prop.Name+" "+
                    prop.PropertyType.Name);
    }
    
    // Start is called before the first frame update
    async Task Start()
    {
        ColyseusClient client = new ColyseusClient("ws://localhost:2567");
        this.CC = client;
        try {
            var room = await client.Join("my_lobby");
            
            Debug.Log("joined successfully");
            room.OnMessage<IndexedDictionary<string, object>[]>("rooms", (message) => {
                Debug.Log ("rooms message");
                Debug.Log(message.Length+" rooms:");
              // GetPropertyValues(message[0]);
                // Debug.Log(message[0].GetType().GetProperty("Item").GetValue(message[0], null));
                // foreach (var key in message[0].Keys)
                // {
                //     Debug.Log(key);
                // }
                // foreach (var v in message[0].Values)
                // {
                //     Debug.Log(v);
                // }
                Debug.Log(message[0]["roomId"]);
                
                // Debug.Log(message[0].Keys);
            });
            room.OnMessage<List<object>>("+", (message) => {
                Debug.Log ("room+ message");
                foreach (var s in message)
                {
                    Debug.Log(s);
                }
            });
            room.OnMessage<string>("-", (message) => {
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
        this.CC.Create<MyRoomState>("my_game");
        Debug.Log("create room!");
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}


internal class RoomsMessage
{
    public string[] rooms;
}
