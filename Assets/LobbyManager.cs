using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte roomSize;

    private void Start()
    {
        PhotonNetwork.SendRate = 25;
        PhotonNetwork.SerializationRate = 25;
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };

        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions,TypedLobby.Default);
       // PhotonNetwork.CountOfRooms;
        //PhotonNetwork.CountOfPlayersInRooms;
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnCreatedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       foreach(Photon.Realtime.RoomInfo room in roomList)
        {
            
        }
    }

}
