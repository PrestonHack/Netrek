using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    public Photon.Realtime.Player[] players;
    public List<Photon.Realtime.Player> playerList;

    [SerializeField]
    private List<string> playerNames;
    [SerializeField]
    private Dictionary<PunTeams.Team, List<Photon.Realtime.Player>> playersPerTeam;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerList = new List<Photon.Realtime.Player>();
        playersPerTeam = new Dictionary<PunTeams.Team, List<Photon.Realtime.Player>>();
    }

    void Start()
    {
        InvokeRepeating("getPlayerList", 5, 5);

    }

    void Update()
    {
        //TeamExtensions.GetTeam(playerList.Find(PhotonNetwork.LocalPlayer.SetCustomProperties))
        //Hashtable shipProperty = new Hashtable(1) { {"Ship", "AS" } };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
       // Photon.Realtime.Player p = PhotonNetwork.LocalPlayer;
       // object shipType = "";
        //p.CustomProperties.TryGetValue("Ship", out shipType);
        //Debug.Log(shipType);
        //PhotonNetwork.LocalPlayer.CustomProperties.Add("Ship", "AS"); easierway to create custom props
        //use local player to get nickname get team and get customproperties 

    }

    private void getPlayerList()
    {
        players = PhotonNetwork.PlayerList;
        playersPerTeam = PunTeams.PlayersPerTeam;
        if(playersPerTeam != null)
        {
            foreach (KeyValuePair<PunTeams.Team, List<Photon.Realtime.Player>> kvp in playersPerTeam)
            {
               // Debug.Log(kvp.Key.ToString() + " " + kvp.Value.ToStringFull());
            }
        }
 

        for (int i = 0; i < players.Length; i++)
        {
            if (!playerNames.Contains(players[i].NickName))
            {
                playerNames.Add(players[i].NickName);
                playerList.Add(players[i]);
                Debug.Log(players[i].ToStringFull());
              

            }
        }           
    }


}
