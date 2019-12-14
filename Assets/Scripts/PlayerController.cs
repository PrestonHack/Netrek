using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string emblem;    
    [SerializeField]
    private SpriteRenderer shipSprite;
    [SerializeField]
    private Vector2 navPoint;
    public Vector2 start;
    public Vector2 end;
    [SerializeField]
    private Vector2 point;
    [SerializeField]
    private float angle;
    [SerializeField]
    private Camera cam;
    public int torpCount;
    public List<TorpedoBehavior> torps = new List<TorpedoBehavior>();
    [SerializeField]
    private GameObject torp;
    [SerializeField]
    private int torpDamage;
    [SerializeField]
    private int torpCost;
    [SerializeField]
    private int torpWeaponTemp;
    public float warpCost;
    public float maxWarp;
    public float warpNumber;
    public float warpPercent;
    public float warpFuelUse;
    public GameObject weapon;
    public bool shieldOn;
    public bool repairOn;
    public bool cloakOn;
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private GameObject cloak;
    [SerializeField]
    private GameObject mapCloak;
    [SerializeField]
    private GameObject mapEmblem;
    [SerializeField]
    private GameObject playerLabel;
    private float rotationSpeed;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float nextFire;
    public float length; 
    [SerializeField]
    private Canvas teamShipCanvas;
    public PhotonView photonView;
    [SerializeField]
    float distance;
    [SerializeField]
    private Collider2D hullCollider;   
    public HullController hullController;
    public FuelController fuelController;
    public SpeedController speedController;
    [SerializeField]
    private TractorController tractorController;
    [SerializeField]
    private PressorController pressorController;
    [SerializeField]
    private PhaserController phaserController;
    [SerializeField]
    private DockController dockController;
    [SerializeField]
    private TemperatureController temperatureController;
    public OrbitController orbitController;
    // Start is called before the first frame update
    void Start()
    {
        repairOn = false;
        warpNumber = 0;
        teamShipCanvas = GameObject.Find("TeamShipCanvas").GetComponent<Canvas>();
        navPoint = Vector2.up;
        photonView = GetComponent<PhotonView>();
        emblem = PhotonNetwork.LocalPlayer.GetTeam().ToString().Substring(0, 1) + PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        photonView.RPC("setEmblemName", RpcTarget.AllBuffered, emblem, PhotonNetwork.LocalPlayer.NickName);
        warpFuelUse = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(navPoint, start);
        //move cam
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
        //movement code
        if (Input.GetMouseButton(1) && !teamShipCanvas.enabled)
        {
            point = cam.ScreenToWorldPoint(Input.mousePosition);
            navPoint = point;
            end = point;
        }
        start = transform.position;
        length = Vector2.Distance(end, start);
        if (length < 0.1f)
        {
            navPoint = transform.position + transform.up;
            end = navPoint;
        }

        if (speedController.desiredSpeed != 0)
        {
            repairOn = false;
        }
        if (repairOn)
        {
            speedController.desiredSpeed = 0;
        }

        if (!dockController.docked && !orbitController.orbiting)
        {
            move();
        }
        else
        {
            speedController.desiredSpeed = 0;
        }

        warpPercent = (speedController.currentSpeed / maxWarp);
        warpFuelUse = warpCost * speedController.desiredSpeed;
        warpNumber = speedController.desiredSpeed;
        rotationSpeed = speedController.rotationSpeed;
      
        //torpedo code
        if (Input.GetKey(KeyCode.T) && Time.time > nextFire && !Input.GetKey(KeyCode.LeftShift) && !cloak.activeInHierarchy && fuelController.currentFuel >= torpCost && torpCount < 8)
        {
            nextFire = Time.time + fireRate;
            torpCount++;
            Vector3 weaponPosition = weapon.transform.position;
            Quaternion weaponRotation = weapon.transform.rotation;
            photonView.RPC("setTorpCount", RpcTarget.All,torpCount);
            photonView.RPC("fireTorp", RpcTarget.AllViaServer, weaponPosition, weaponRotation, this.gameObject.layer, torpDamage);
            fuelController.currentFuel -= torpCost;
            temperatureController.currentWeaponTemp += torpWeaponTemp / 10;
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("detOwnTorp",RpcTarget.AllBufferedViaServer);
        }

        //shield code
        if ((Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.S)) && (hullController.shieldHealth > 0 && !repairOn))
        {
            photonView.RPC("activateShield", RpcTarget.AllBufferedViaServer);
        }
        if((fuelController.currentFuel <= fuelController.shieldFuelCost) && shieldOn)
        {
            shieldOn = false;
            photonView.RPC("activateShield", RpcTarget.AllBufferedViaServer);
        }
        //repair code
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            if (!repairOn)
            {
                repairOn = true;
                speedController.desiredSpeed = 0;
                photonView.RPC("deactivateShield", RpcTarget.AllBufferedViaServer);
            }
            else
            {
                repairOn = false;
            }
        }
        //cloak code
        if (Input.GetKeyDown(KeyCode.C))
        {
            int layer;

            if (cloak.activeInHierarchy)
            {
                cloak.SetActive(false);
                cloakOn = false;
                shipSprite.enabled = true;
                layer = this.gameObject.layer;
            }
            else
            {
                cloak.SetActive(true);
                cloakOn = true;
                phaserController.turnOff();
                pressorController.off();
                tractorController.off();
                shipSprite.enabled = false;
                if(this.gameObject.layer == 10)
                {
                    layer = 26;
                }
                else if(this.gameObject.layer == 11)
                {
                    layer = 27;
                }
                else if (this.gameObject.layer == 12)
                {
                    layer = 28;
                }
                else
                {
                    layer = 29;
                }
            }
            photonView.RPC("activateCloak", RpcTarget.OthersBuffered, cloakOn, layer);
        }
        if (cloakOn && fuelController.currentFuel < fuelController.cloakFuelCost)
        {
            cloak.SetActive(false);
            cloakOn = false;
            shipSprite.enabled = true;
            photonView.RPC("activateCloak", RpcTarget.OthersBuffered, cloakOn, this.gameObject.layer);
        }
    }

    [PunRPC]
    public void setEmblemName(string em, string nickname)
    {
        gameObject.transform.root.gameObject.name = nickname;
        emblem = em;
        playerLabel.GetComponentInChildren<TMP_Text>().text = em;
        mapEmblem.GetComponentInChildren<TMP_Text>().text = em;
    }

    [PunRPC]
    public void activateCloak(bool on, int layer)
    {
        if(on)
        {
            mapCloak.SetActive(true);
            playerLabel.SetActive(false);
            mapEmblem.SetActive(false);
            shipSprite.enabled = false;
            cloak.SetActive(true);
            shield.layer = layer;
        }
        else
        {  
            mapCloak.SetActive(false);
            playerLabel.SetActive(true);
            mapEmblem.SetActive(true);
            shipSprite.enabled = true;
            cloak.SetActive(false);
            shield.layer = layer;
        }
    }

    [PunRPC]
    public void activateShield()
    {
        if (shield.activeInHierarchy)
        {
            shieldOn = false;
            shield.SetActive(false);
            hullCollider.enabled = true;
        }
        else
        {
            shieldOn = true;
            shield.SetActive(true);
            hullCollider.enabled = false;
        }
    }

    [PunRPC]
    public void deactivateShield()
    {
        shieldOn = false;
        shield.SetActive(false);
        hullCollider.enabled = true;
    }

    [PunRPC]
    public void detOwnTorp()
    {
        if(torpCount != 0)
        {
            torps[0].detonate();
            torps.RemoveAt(0);
            torpCount--;
        }
    }

    [PunRPC]
    public void setTorpCount(int count)
    {
        torpCount = count;
    }

    [PunRPC]
    public void fireTorp(Vector3 position, Quaternion rotation, int layer, int damage)
    {        
        if(layer == 10)
        {
            torp = PoolManager.Instance.fedRequestTorp();
        }
        else if(layer == 11)
        {
            torp = PoolManager.Instance.kliRequestTorp();
        }
        else if (layer == 12)
        {
            torp = PoolManager.Instance.oriRequestTorp();
        }
        else if (layer == 13)
        {
            torp = PoolManager.Instance.romRequestTorp();
        }
        
        TorpedoBehavior torpBehavior = torp.GetComponent<TorpedoBehavior>();
        torpBehavior.damage = damage;
        torpBehavior.playerController = this;
        torp.transform.position = position;
        torp.transform.rotation = rotation;        
        torp.SetActive(true);
        torps.Add(torpBehavior);
    }

    public void move()
    {
        playerSpeed = speedController.moveSpeed;
        RotateTowards(navPoint);
        transform.position += transform.up* playerSpeed * Time.deltaTime;
    }

    private void RotateTowards(Vector2 target)
    {                   
        float offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.gameObject.name.ToString() + "  ontrigger_player");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log(col.transform.gameObject.name.ToString() + "  ontriggerstay_player");

        if (col.transform.root.name != this.gameObject.transform.root.name && col.gameObject.name == "tractor")
        {
            this.gameObject.transform.position -= (this.gameObject.transform.position - col.transform.position).normalized * 0.0015f;
            orbitController.orbiting = false;
        }
        if (col.transform.root.name != this.gameObject.transform.root.name && col.gameObject.name == "pressor")
        {
            this.gameObject.transform.position -= (col.transform.position - this.gameObject.transform.position).normalized * 0.0015f;
            orbitController.orbiting = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
    }
}
