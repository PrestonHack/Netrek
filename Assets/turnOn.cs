using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            mapCam.enabled = true;
            playerCam.enabled = true;
            pc.enabled = true;
            al.enabled = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
