using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private RoomOptions roomOptions;
    [SerializeField]
    private byte ticks;
    [SerializeField]
    private byte roomSize;
    [SerializeField]
    private float ping;
    [SerializeField]
    private string currentCluster;
    [SerializeField]
    private bool isConnected;
    [SerializeField]
    private string clientState;
    [SerializeField]
    private string server;
    [SerializeField]
    private byte numOfPlayers;
    [SerializeField]
    private Text clientStateText;
    [SerializeField]
    private Text pingText;
    [SerializeField]
    private Text numberOfPlayersText;

    private void Start()
    {

        PhotonNetwork.SendRate = ticks;
        PhotonNetwork.SerializationRate = ticks;
        //PhotonNetwork.ConnectUsingSettings(); calling this on the login button in UIcontroller
        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating("getNetworkInfo", 0, 5);

    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, TypedLobby.Default);
        PhotonNetwork.AutomaticallySyncScene = true;

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

    void Update()
    {
        
    }

    private void getNetworkInfo()
    {
        ping = PhotonNetwork.GetPing();
        currentCluster = PhotonNetwork.CurrentCluster;
        isConnected = PhotonNetwork.IsConnected;
        clientState = PhotonNetwork.NetworkClientState.ToString();
        server = PhotonNetwork.Server.ToString();
        numOfPlayers = (byte)PhotonNetwork.CountOfPlayers;
        clientStateText.text = string.Format("Connection Status: {0} ", clientState);
        pingText.text = "Ping: " + ping.ToString();
        numberOfPlayersText.text = string.Format("Players:{0} ", numOfPlayers.ToString());
    }
}
