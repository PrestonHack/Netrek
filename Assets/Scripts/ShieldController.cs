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
    [SerializeField]
    private PlayerController playerController;


    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        pV = this.gameObject.GetPhotonView();
    }

    private void Update()
    {
        if(hullController.shieldHealth <= 0)
        {
            pV.RPC("shieldFailure", RpcTarget.AllBuffered);
        }    
    }

    [PunRPC]
    public void shieldFailure()
    {
        playerController.shieldOn = false;
        this.gameObject.SetActive(false);
        hullCollider.enabled = true;
    }

    private void torpDamage(Collider2D collision)
    {
        float dmg = collision.gameObject.GetComponentInParent<torpedoBehavior>().damage;
        float shieldHealth = hullController.shieldHealth;
        if (shieldHealth >= dmg)
        {
            shieldHealth -= dmg;
        }
        else
        {
            float diff = dmg - shieldHealth;
            float shieldDmg = dmg - diff;
            shieldHealth -= shieldDmg;
            hullController.hullHealth -= diff;
        }
        hullController.shieldHealth -= collision.gameObject.GetComponentInParent<torpedoBehavior>().damage;
        collision.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void phaserDamage(Collider2D collision)
    {
        Debug.Log("phaser hit shield");
        float dmg = collision.gameObject.GetComponentInParent<PhaserController>().damage;
        float shieldHealth = hullController.shieldHealth;
        if (shieldHealth >= dmg)
        {
            shieldHealth -= dmg;
            Debug.Log("phaser hit shield for: " + dmg + "remaining shield health: " + shieldHealth + "hit by: " + collision.transform.root.gameObject.name);
        }
        else
        {
            float diff = dmg - shieldHealth;
            float shieldDmg = dmg - diff;
            shieldHealth -= shieldDmg;
            hullController.hullHealth -= diff;
            Debug.Log("phaser hit shield for: " + shieldDmg + "remaining shield health: " + shieldHealth + "Phaser hit hull for remaining damge of: " + diff);
        }
        hullController.shieldHealth = shieldHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Regex.IsMatch(collision.gameObject.name, "phaser"))
        {
            phaserDamage(collision);
        }
        if (Regex.IsMatch(collision.gameObject.name, "torp"))
        {
            torpDamage(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.root.name != this.gameObject.transform.root.name && collision.gameObject.name == "tractor")
        {
            this.gameObject.transform.parent.parent.position -= (this.gameObject.transform.position - collision.transform.position).normalized * 0.0015f;
        }
        if (collision.transform.root.name != this.gameObject.transform.root.name && collision.gameObject.name == "pressor")
        {
            this.gameObject.transform.parent.parent.position -= (collision.transform.position - this.gameObject.transform.position).normalized * 0.0015f;
        }
       
    }



}
