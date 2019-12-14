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

    private void torpDamage(Collider2D collision)
    {
        TorpedoBehavior torpedoBehavior = collision.transform.parent.GetComponent<TorpedoBehavior>();
        float dmg = torpedoBehavior.damage;
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
        hullController.shieldHealth -= torpedoBehavior.damage;
        torpedoBehavior.gameObject.SetActive(false);

        torpedoBehavior.playerController.torpCount--;
        torpedoBehavior.playerController.torps.Remove(torpedoBehavior);
        
    }

    private void phaserDamage(Collider2D collision)
    {
        PhaserController phaserController = collision.GetComponent<PhaserController>();
        float dmg = phaserController.damage;
        float shieldHealth = hullController.shieldHealth;
        float diff = 0;
        float shieldDmg = dmg;
        if (dmg > shieldHealth)
        {
            diff = dmg - shieldHealth;
            shieldDmg = dmg - diff;
            phaserController.damage = diff;
            dmg = phaserController.damage;
        }
        hullController.shieldHealth -= shieldDmg;
        hullController.hullHealth -= diff;
        phaserController.damage = 0;
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
            playerController.orbitController.orbiting = false;
        }
        if (collision.transform.root.name != this.gameObject.transform.root.name && collision.gameObject.name == "pressor")
        {
            this.gameObject.transform.parent.parent.position -= (collision.transform.position - this.gameObject.transform.position).normalized * 0.0015f;
            playerController.orbitController.orbiting = false;
        }
    }



}
