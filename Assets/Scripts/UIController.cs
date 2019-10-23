using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Photon.Pun.UtilityScripts;
using System;


public class UIController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text userNameInput;
    [SerializeField]
    private Canvas loginCanvas;
    [SerializeField]
    private Canvas teamShipCanvas;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private GameObject shipInstance;
    [SerializeField]
    private PhotonView pv;
    

    public void onLoginButtonClick()
    {
        PhotonNetwork.LocalPlayer.NickName = userNameInput.text;
        loginCanvas.gameObject.SetActive(false);
        teamShipCanvas.gameObject.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (teamShipCanvas.enabled)
            {
                teamShipCanvas.enabled = false;
            }
            else
            {
                teamShipCanvas.enabled = true;
            }
        }

        if(!string.IsNullOrWhiteSpace(userNameInput.text))
        {
            loginButton.interactable = true;
        }
        else
        {
            loginButton.interactable = false;
        }
    }

    public void onReadyClick()
    {
        object shipType = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ship", out shipType);
        shipInstance = PhotonNetwork.Instantiate(Path.Combine("ships", PhotonNetwork.LocalPlayer.GetTeam().ToString(), "Player_" + PhotonNetwork.LocalPlayer.GetTeam().ToString() + "_" + shipType.ToString()), Vector3.zero, Quaternion.identity);
        teamShipCanvas.enabled = false;
        shipInstance.gameObject.name = PhotonNetwork.LocalPlayer.NickName.ToString();
        
    }    

}
