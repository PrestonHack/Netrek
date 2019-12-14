using UnityEngine;
using System.Text.RegularExpressions;
using Photon.Pun;

public class HullController : MonoBehaviour
{
    public float hullMaxHealth;
    public float hullHealth;
    public float hullPercent;
    public float shieldMaxHealth;
    public float shieldHealth;
    public float shieldPercent;
    [SerializeField]
    private CircleCollider2D shieldCollider;
    [SerializeField]
    private PolygonCollider2D hullCollider;
    [SerializeField]
    private Animator explosionAnimator;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject shipSprite;
    [SerializeField]
    TorpedoBehavior torpBehavior;
    [SerializeField]
    private PhotonView pV;
    // Start is called before the first frame update
    void Start()
    {
        pV = this.gameObject.GetComponent<PhotonView>();
        hullHealth = hullMaxHealth;
        shieldHealth = shieldMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hullHealth = Mathf.Clamp(hullHealth, 0, hullMaxHealth);
        shieldHealth = Mathf.Clamp(shieldHealth, 0, shieldMaxHealth);
        hullPercent = (hullHealth / hullMaxHealth);
        shieldPercent = (shieldHealth / shieldMaxHealth);        
        
        if(hullHealth <= 0)
        {
            pV.RPC("die", RpcTarget.All);
        }

        
        if (explosionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !explosionAnimator.IsInTransition(0))
        {
            pV.RPC("endDie", RpcTarget.All);
        }
    }
    [PunRPC]
    public void endDie()
    {
        explosion.SetActive(false);
        if (pV.IsMine)
        {
            PhotonNetwork.Destroy(this.transform.root.gameObject);
        }
        else
        {
            Destroy(this.transform.root.gameObject);
        }
    }

    [PunRPC]
    public void die()
    {
        shipSprite.SetActive(false);
        explosion.SetActive(true);        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("HullController_TriggerEnter: " + other.gameObject.name);
        if (!shieldCollider.gameObject.activeInHierarchy)
        {
            if (Regex.IsMatch(other.gameObject.name, "torp"))
            {
                Debug.Log("TORP!");
                torpBehavior = other.gameObject.GetComponentInParent<TorpedoBehavior>();
                hullHealth -= torpBehavior.damage;
                torpBehavior.playerController.torpCount--;
                torpBehavior.playerController.torps.Remove(torpBehavior);
                other.gameObject.transform.parent.gameObject.SetActive(false);
            }
            if (Regex.IsMatch(other.gameObject.name, "phaser"))
            {
                hullHealth -= other.gameObject.GetComponentInParent<PhaserController>().damage;
            }
        }
    } 
}
