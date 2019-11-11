using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private int numOfPlayers;
    [SerializeField]
    private int ping;
    [SerializeField]
    private Text pingText;
    [SerializeField]
    private Text regionText;
    [SerializeField]
    private Text clientStateText;
    [SerializeField]
    private Text numberOfPlayers;
    [SerializeField]
    private string currentCluster;
    [SerializeField]
    private bool isConnected;
    [SerializeField]
    private string clientState; 
    [SerializeField]
    private string server;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SendRate = 23;
        PhotonNetwork.SerializationRate = 23;
        PhotonNetwork.ConnectUsingSettings();
        InvokeRepeating("updatePing", 2, 6);
    }
    public override void OnConnectedToMaster()
    {
        regionText.text = "Region: " + PhotonNetwork.CloudRegion;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void updatePing()
    {        
        ping = PhotonNetwork.GetPing();
        currentCluster = PhotonNetwork.CurrentCluster;
        isConnected = PhotonNetwork.IsConnected;
        clientState = PhotonNetwork.NetworkClientState.ToString();
        server = PhotonNetwork.Server.ToString();
        numOfPlayers = PhotonNetwork.CountOfPlayers;
        clientStateText.text = "Connection Status: " + clientState;
        pingText.text = "Ping: " + ping.ToString();
        numberOfPlayers.text = "Players: " + numOfPlayers.ToString();
    }
}
