using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;

public class ShieldController : MonoBehaviour
{
    [SerializeField]
    private PhotonView pV;
    [SerializeField]
    private HullController hullController;
    [SerializeField]
    private PolygonCollider2D hullCollider;


    private void Start()
    {
        pV = this.gameObject.GetPhotonView();
    }

    private void Update()
    {
        if(hullController.shieldHealth <= 0)
        {
            pV.RPC("shieldFailure", RpcTarget.AllBuffered);
            hullController.hullHealth += hullController.shieldHealth;
        }    
    }

    [PunRPC]
    public void shieldFailure()
    {
        this.gameObject.SetActive(false);
        hullCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Regex.IsMatch(collision.gameObject.name, "phaser"))
        {
            Debug.Log("phaser hit shield");
            hullController.shieldHealth -= collision.gameObject.GetComponentInParent<PhaserController>().damage;
        }
        if (Regex.IsMatch(collision.gameObject.name, "torp"))
        {
            hullController.shieldHealth -= collision.gameObject.GetComponentInParent<torpedoBehavior>().damage;
            collision.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.root.name != this.gameObject.transform.root.name && col.gameObject.name == "tractor")
        {
            this.gameObject.transform.parent.parent.position -= (this.gameObject.transform.position - col.transform.position).normalized * 0.0015f;

        }
        if (col.transform.root.name != this.gameObject.transform.root.name && col.gameObject.name == "pressor")
        {
            this.gameObject.transform.parent.parent.position -= (col.transform.position - this.gameObject.transform.position).normalized * 0.0015f;


        }
    }



}
