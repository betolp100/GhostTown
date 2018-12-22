﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 16;

    private string roomName;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName (string _name)
    {
        roomName = _name;
        Debug.Log(_name);
    }

    public void CreateRoom()
    {
        if (roomName!="" && roomName!=null)
        {
            Debug.Log("Creating Room: "+roomName+ " with room for: "+roomSize+" players.");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }

}
