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
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private GameObject dashManager;
    [SerializeField]
    private DockController dockController;
    [SerializeField]
    private OrbitController orbitController;
    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            orbitController.enabled = true;
            dockController.enabled = true;
            mapCam.enabled = true;
            playerCam.enabled = true;
            pc.enabled = true;
            al.enabled = true;
            playerUI.SetActive(true);
            dashManager.SetActive(true);
        }
    }

}
