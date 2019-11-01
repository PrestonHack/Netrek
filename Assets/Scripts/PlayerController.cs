﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Photon.Pun.UtilityScripts;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string emblem;
    [SerializeField]
    private SpriteRenderer shipSprite;
    [SerializeField]
    private Vector3 crosshairPosition;
    [SerializeField]
    private Vector2 navPoint;
    [SerializeField]
    private Vector2 crosshairPoint;
    [SerializeField]
    private float angle;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private FuelController fuelController;
    [SerializeField]
    private TemperatureController temperatureController;
    [SerializeField]
    private int torpDamage;
    [SerializeField]
    private int torpCost;
    [SerializeField]
    private int torpWeaponTemp;
    [SerializeField]
    private int warpCost;
    public int warpFuelUse;
    [SerializeField]
    private GameObject torp;
    [SerializeField]
    private Vector2 crosshairDebug;
    public GameObject weapon;
    public bool shieldOn;
    [SerializeField]
    private GameObject shield;
    public bool cloakOn;
    [SerializeField]
    private GameObject cloak;
    [SerializeField]
    private GameObject mapCloak;
    [SerializeField]
    private GameObject mapEmblem;
    [SerializeField]
    private GameObject playerLabel;
    [SerializeField]
    private Vector3 navPosition;
    [SerializeField]
    private Vector3 point;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float playerSpeed;
    public float warpNumber;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float nextFire;
    public Vector3 start;
    public Vector3 end;
    public float length;
    [SerializeField]
    Vector2 currentPosition;
    [SerializeField]
    Vector2 lastPosition;
    [SerializeField]
    Vector2 velocity;
    [SerializeField]
    private Canvas teamShipCanvas;
    public PhotonView pv;
    [SerializeField]
    float distance;
    [SerializeField]
    private Collider2D hullCollider;
    [SerializeField]
    private HullController hullController;
    // Start is called before the first frame update
    void Start()
    {
        warpNumber = 0;
        teamShipCanvas = GameObject.Find("TeamShipCanvas").GetComponent<Canvas>();
        navPoint = Vector2.up;
        pv = GetComponent<PhotonView>();
        emblem = PhotonNetwork.LocalPlayer.GetTeam().ToString().Substring(0, 1) + PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        pv.RPC("setEmblemName", RpcTarget.AllBuffered, emblem, PhotonNetwork.LocalPlayer.NickName);
        warpFuelUse = 0;
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector2.Distance(navPoint, start);
        currentPosition = transform.position;
        //move cam
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
        //movement code
        crosshairPosition = Input.mousePosition;
        crosshairPoint = cam.ScreenToWorldPoint(new Vector3(crosshairPosition.x, crosshairPosition.y, crosshairPosition.z));
        crosshairPoint = new Vector2(crosshairPoint.x, crosshairPoint.y);
        crosshairDebug = new Vector2(crosshairPoint.x, crosshairPoint.y);

        if (Input.GetMouseButton(1) && !teamShipCanvas.enabled)
        {
            navPosition = Input.mousePosition;
            point = cam.ScreenToWorldPoint(new Vector3(navPosition.x, navPosition.y, navPosition.z));
            navPoint = new Vector2(point.x, point.y);
            //Debug.Log("Navigation point set to: " + navPoint);
            end = new Vector3(point.x, point.y);
        }
        start = new Vector3(transform.position.x, transform.position.y);
        length = Vector3.Distance(end, start);
        if (length < 0.1f)
        {
            navPoint = new Vector3(transform.position.x, transform.position.y, 0) + transform.up;
            end = navPoint;
        }
        Debug.DrawLine(start, end, Color.yellow);
        move();

        //torpedo code
        if (Input.GetKey(KeyCode.T) && Time.time > nextFire && !Input.GetKey(KeyCode.LeftShift) && !cloak.activeInHierarchy && fuelController.currentFuel >= torpCost)
        {
            nextFire = Time.time + fireRate;
            Vector3 weaponPosition = weapon.transform.position;
            Quaternion weaponRotation = weapon.transform.rotation;
            pv.RPC("fireTorp", RpcTarget.AllViaServer, weaponPosition, weaponRotation, this.gameObject.layer, torpDamage);
            fuelController.currentFuel -= torpCost;
            temperatureController.currentWeaponTemp += torpWeaponTemp / 10;

        }
        Debug.DrawLine(start, crosshairDebug);

        //shield code
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.S) && hullController.shieldHealth > 0)
        {
            pv.RPC("activateShield", RpcTarget.AllBufferedViaServer);
        }

        //cloak code
        if (Input.GetKeyDown(KeyCode.C))
        {
            int layer;

            if (cloak.activeInHierarchy)
            {
                cloak.SetActive(false);
                cloakOn = false;
                weapon.SetActive(true);
                shipSprite.enabled = true;
                if (this.gameObject.layer == 10)
                {
                    layer = 10;
                }
                else if (this.gameObject.layer == 11)
                {
                    layer = 11;
                }
                else if (this.gameObject.layer == 12)
                {
                    layer = 12;
                }
                else
                {
                    layer = 13;
                }

            }
            else
            {
                cloak.SetActive(true);
                cloakOn = true;
                weapon.SetActive(false);
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
            pv.RPC("activateCloak", RpcTarget.OthersBuffered, cloakOn, layer);
        }

    }

    [PunRPC]
    public void setEmblemName(string em, string nickname)
    {
        this.gameObject.transform.root.gameObject.name = nickname;
        playerLabel.GetComponentInChildren<TMP_Text>().text = em;
        mapEmblem.GetComponentInChildren<TMP_Text>().text = em;
    }

    [PunRPC]
    public void activateCloak(bool on, int layer)
    {
        if (on)
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
        torp.GetComponent<torpedoBehavior>().damage = damage;
        torp.transform.position = position;
        torp.transform.rotation = rotation;
        torp.SetActive(true);
            
    }

    public void move()
    {
        playerSpeed = getMoveSpeed();
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime *0.2f);
    }

    private float getMoveSpeed()
    {

        if (Input.GetKey(KeyCode.Alpha1))
        {
            playerSpeed = 0.025f;
            rotationSpeed = 5;
            warpFuelUse = warpCost * 1;
            warpNumber = 1;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            playerSpeed = 0.05f;
            rotationSpeed = 4;
            warpFuelUse = warpCost * 2;
            warpNumber = 2;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            playerSpeed = 0.075f;
            rotationSpeed = 3;
            warpFuelUse = warpCost * 3;
            warpNumber = 3;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            playerSpeed = 0.15f;
            rotationSpeed = 2;
            warpFuelUse = warpCost * 4;
            warpNumber = 4;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            playerSpeed = 0.2f;
            rotationSpeed = 1;
            warpFuelUse = warpCost * 5;
            warpNumber = 5;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            playerSpeed = 0.25f;
            rotationSpeed = 0.9f;
            warpFuelUse = warpCost * 6;
            warpNumber = 6;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            playerSpeed = 0.3f;
            rotationSpeed = 0.8f;
            warpFuelUse = warpCost * 7;
            warpNumber = 7;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            playerSpeed = 0.35f;
            rotationSpeed = 0.7f;
            warpFuelUse = warpCost * 8;
            warpNumber = 8;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            playerSpeed = 0.4f;
            rotationSpeed = 0.6f;
            warpFuelUse = warpCost * 9;
            warpNumber = 9;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            playerSpeed = 0;
            rotationSpeed = 5;
            warpFuelUse = warpCost * 0;
            warpNumber = 0;
            return playerSpeed;
        }
        return playerSpeed;
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

        }
        if (col.transform.root.name != this.gameObject.transform.root.name && col.gameObject.name == "pressor")
        {
            this.gameObject.transform.position -= (col.transform.position - this.gameObject.transform.position).normalized * 0.0015f;


        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.gameObject.name.ToString() + "oncollision_player");

    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

}
