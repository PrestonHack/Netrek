using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class roominfo : MonoBehaviour
{
    [SerializeField]
    private Text players;

    public void getPlayers()
    {
        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            players.text += p.NickName.ToString() + "\n";
        }
        players.text += PhotonNetwork.CurrentRoom.Name;
    }

}
