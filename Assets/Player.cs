using UnityEngine;
using Photon.Pun;
using System.IO;

public class Player : MonoBehaviour
{
    private string user;
    private string team;
    private string ship;

    public Player(string username, string team, string ship)
    {
        this.user = username;
        this.team = team;
        this.ship = ship;
    }    

    public void spawnPlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("ships", "Fed", "testplayer"), Vector3.zero, Quaternion.identity);
        
    }
 
}
