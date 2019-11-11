using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class TeamManager : Photon.Pun.MonoBehaviourPun
{
    [SerializeField]
    private CanvasRenderer fedShipPanel;
    [SerializeField]
    private CanvasRenderer kliShipPanel;
    [SerializeField]
    private CanvasRenderer oriShipPanel;
    [SerializeField]
    private CanvasRenderer romShipPanel;

    public void OnFederationButtonClick()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.Fed);
            fedShipPanel.gameObject.SetActive(true);
            kliShipPanel.gameObject.SetActive(false);
            oriShipPanel.gameObject.SetActive(false);
            romShipPanel.gameObject.SetActive(false);
            Debug.Log("team: " + PhotonNetwork.LocalPlayer.GetTeam().ToString());
        }
    }
    public void OnKlingonButtonClick()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.Kli);
            fedShipPanel.gameObject.SetActive(false);
            kliShipPanel.gameObject.SetActive(true);
            oriShipPanel.gameObject.SetActive(false);
            romShipPanel.gameObject.SetActive(false);
            Debug.Log("team: " + PhotonNetwork.LocalPlayer.GetTeam().ToString());
        }
    }
    public void OnOrionButtonClick()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.Ori);
            fedShipPanel.gameObject.SetActive(false);
            kliShipPanel.gameObject.SetActive(false);
            oriShipPanel.gameObject.SetActive(true);
            romShipPanel.gameObject.SetActive(false);
            Debug.Log("team: " + PhotonNetwork.LocalPlayer.GetTeam().ToString());
        }
    }
    public void OnRomulanButtonClick()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.Rom);
            fedShipPanel.gameObject.SetActive(false);
            kliShipPanel.gameObject.SetActive(false);
            oriShipPanel.gameObject.SetActive(false);
            romShipPanel.gameObject.SetActive(true);
            Debug.Log("team: " + PhotonNetwork.LocalPlayer.GetTeam().ToString());
        }
    }

}
