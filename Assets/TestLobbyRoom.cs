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
using UnityEngine.UIElements;
using Object = System.Object;
using Type = System.Type;


public class TestLobbyRoom : MonoBehaviour
{
    public GameObject IconPrefab;
    public Transform sv;
    public Text RoomsCounter;
    private ColyseusClient CC;
    private List<IndexedDictionary<string, object>> Rooms;
    private List<GameObject> icons = new List<GameObject>();

    private static void GetPropertyValues(Object obj)
    {
        Type t = obj.GetType();
        Debug.Log("Type is: " + t.Name);
        PropertyInfo[] props = t.GetProperties();
        Debug.Log("Properties (N): " +
                  props.Length);
        foreach (var prop in props)
            if (prop.GetIndexParameters().Length == 0)
                Debug.Log("    " + prop.Name + " " +
                          prop.PropertyType.Name + " " +
                          prop.GetValue(obj));
            else
                Debug.Log("   " + prop.Name + " " +
                          prop.PropertyType.Name);
    }

    // Start is called before the first frame update
    async Task Start()
    {
        ColyseusClient client = new ColyseusClient("ws://localhost:2567");
        this.CC = client;
        try
        {
            var room = await client.Join("my_lobby");

            Debug.Log("lobby joined successfully");
            room.OnMessage<IndexedDictionary<string, object>[]>("rooms", (message) =>
            {
                Debug.Log("rooms message");
                Debug.Log(message.Length + " rooms:");
                Rooms = new List<IndexedDictionary<string, object>>(message);
                // Debug.Log(message[0]["roomId"]);
                UpdateRooms();
            });
            room.OnMessage<List<object>>("+", (message) =>
            {
                Debug.Log("room+ message");
                var rn = message[0].ToString();
                var found = false;
                foreach (var r in Rooms)
                {
                    if (r["roomId"].ToString() == rn)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var room = new IndexedDictionary<string, object>();
                    room.Add("roomId", message[0].ToString());
                    Rooms.Add(room);
                    UpdateRooms();
                }
                
            });
            room.OnMessage<string>("-", (message) =>
            {
                Debug.Log("room- message");
                Debug.Log(message);
                for (int i = 0; i < Rooms.Count; i++)
                {
                    var r = Rooms[i];
                    if (r["roomId"].ToString() == message.ToString())
                    {
                        Rooms.RemoveAt(i);
                        break;
                    }
                }
                UpdateRooms();
            });
        }
        catch (Exception ex)
        {
            Debug.Log("join error");
            Debug.Log(ex.Message);
        }
    }

    public void CreateRoom()
    {
        this.CC.Create<MyRoomState>("my_game");
        Debug.Log("create room!");
    }

    public void UpdateRooms()
    {
        foreach (var icon in icons)
        {
            GameObject.Destroy(icon);
        }
        icons.Clear();
        for (int i = 0; i < Rooms.Count; i++)
        {
            GameObject icon = Instantiate(IconPrefab, sv, false);
            icon.GetComponentInChildren<Text>().text = i.ToString();
            icons.Add(icon);
        }

        RoomsCounter.text = "房间数量: " + Rooms.Count;
        Debug.Log("rooms update!");
    }
}

