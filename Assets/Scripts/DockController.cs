using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DockController : MonoBehaviour
{
    public bool docked;
    public bool dockingEnabled;
    [SerializeField]
    private DockManager dockManager;
    [SerializeField]
    private DockEndPoint dock;
    [SerializeField]
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.root.name + " is inside the docking collider: DockController");
        dockingEnabled = true;
        dockManager = collision.GetComponentInChildren<DockManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dockingEnabled = false;
    }
    private void Update()
    {
        if (docked && dockingEnabled)
        {
            this.transform.parent.parent.transform.position = dock.transform.position;
            this.transform.parent.parent.transform.rotation = dock.transform.rotation;
        }
        else if(docked && !dockingEnabled)
        {
            docked = false;
            dock.occupied = false;
        }

        if (dockingEnabled && !docked && Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("findDock", RpcTarget.AllBufferedViaServer);
        }
        else if(docked && Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("unDock", RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    public void unDock()
    {
        Debug.Log("Dock disengaged");
        docked = false;
        dock.occupied = false;
    }

    [PunRPC]
    public void findDock()
    {
        foreach (DockEndPoint d in dockManager.docks)
        {
            if (!d.occupied)
            {
                d.occupied = true;
                dock = d;
                docked = true;
                Debug.Log("Dock engaged");
                break;
            }
        }
    }
}
