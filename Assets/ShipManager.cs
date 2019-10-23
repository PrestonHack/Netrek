using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private Button readyButton;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void ASButtonClick()
    {
        ExitGames.Client.Photon.Hashtable shipProperty = new ExitGames.Client.Photon.Hashtable(1) { {"Ship", "AS" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        Debug.Log(shipType.ToString());
        readyButton.interactable = true;
    }
    public void BBButtonClick()
    {
        ExitGames.Client.Photon.Hashtable shipProperty = new ExitGames.Client.Photon.Hashtable(1) { { "Ship", "BB" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        Debug.Log(shipType.ToString());
        readyButton.interactable = true;
    }
    public void CAButtonClick()
    {
        ExitGames.Client.Photon.Hashtable shipProperty = new ExitGames.Client.Photon.Hashtable(1) { { "Ship", "CA" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        Debug.Log(shipType.ToString());
        readyButton.interactable = true;
    }
    public void DDButtonClick()
    {
        ExitGames.Client.Photon.Hashtable shipProperty = new ExitGames.Client.Photon.Hashtable(1) { { "Ship", "DD" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        Debug.Log(shipType.ToString());
        readyButton.interactable = true;
    }
    public void SBButtonClick()
    {
        ExitGames.Client.Photon.Hashtable shipProperty = new ExitGames.Client.Photon.Hashtable(1) { { "Ship", "SB" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        Debug.Log(shipType.ToString());
        readyButton.interactable = true;
    }
    public void SCButtonClick()
    {
        ExitGames.Client.Photon.Hashtable shipProperty = new ExitGames.Client.Photon.Hashtable(1) { { "Ship", "SC" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(shipProperty);
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        Debug.Log(shipType.ToString());
        readyButton.interactable = true;
    }
}
