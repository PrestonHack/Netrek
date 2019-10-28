using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnOn : MonoBehaviour
{
    [SerializeField]
    private Camera[] cameras;
    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private Camera mapCam;
    [SerializeField]
    private PlayerController pc;
    [SerializeField]
    private Photon.Pun.PhotonView pv;
    [SerializeField]
    AudioListener al;
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private GameObject dashManager;
    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            mapCam.enabled = true;
            playerCam.enabled = true;
            pc.enabled = true;
            al.enabled = true;
            playerUI.SetActive(true);
            dashManager.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
