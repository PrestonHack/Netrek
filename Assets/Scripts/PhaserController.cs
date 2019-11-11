using System.Collections;
using UnityEngine;
using Photon.Pun;
using System;

public class PhaserController : MonoBehaviour
{
    public bool phaserOn;
    [SerializeField]
    public float currentLength;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D phaserCollider;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float angle;
    [SerializeField]
    private AudioSource soundClip;
    [SerializeField]
    private Vector2 mousePosition;
    [SerializeField]
    private Vector2 point;
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private Vector2 start;
    public Vector2 end;
    [SerializeField]
    public float distance;
    [SerializeField]
    private float maxRange = 1.75f;
    [SerializeField]
    private Vector2 length;
    public float phaserLengthPercent;
    [SerializeField]
    private FuelController fuelController;
    [SerializeField]
    private TemperatureController temperatureController;
    [SerializeField]
    private int phaserWeaponTemp;
    [SerializeField]
    private int phaserCost;
    [SerializeField]
    private float phaserDamage;
    public float damage;
    public float fireRate = 1;
    public float nextTime;
    public int layerMask;

    private void Start()
    {
        point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
        soundClip = GetComponent<AudioSource>();
        layerMask = LayerMask.GetMask("Fed", "Kli", "Ori", "Rom") &~ LayerMask.GetMask(LayerMask.LayerToName(this.transform.root.gameObject.layer));
    }

    void Update()
    {      

        if (Input.GetMouseButtonDown(0) && photonView.IsMine && fuelController.currentFuel >= phaserCost && Time.time > nextTime && !phaserOn && !fuelController.playerController.cloakOn)
        {
            if (photonView.IsMine)
            {
                nextTime = Time.time + fireRate;
                mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                direction = mousePosition - new Vector2(transform.position.x, transform.position.y);
                distance = Mathf.Clamp(Vector2.Distance(transform.position, mousePosition), 0, maxRange);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);

                if (hit.collider != null)
                { 
                    end = hit.collider.transform.position;
                }
                else
                { 
                    end = new Vector2(transform.position.x, transform.position.y) + (direction.normalized * distance);

                }
                distance = Mathf.Clamp(Vector2.Distance(transform.position, end), 0, maxRange);
                phaserOn = true;
                subtractCost();
                damage = getDamage(distance, phaserDamage);
                Debug.Log("Phaser damage: " + damage + " line length %: " + (distance/maxRange));

                photonView.RPC("firePhaserRPC", RpcTarget.AllViaServer, end, damage);
            }
        }

        if (photonView.IsMine)
        {
            currentLength = Vector2.Distance(transform.position, end);
            if (phaserOn && currentLength > maxRange)
            {
                photonView.RPC("turnOff", RpcTarget.All);
            }
        }

        lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        if (phaserOn)
        {
            Vector2 size = phaserCollider.size;
            size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
            phaserCollider.size = size;
            phaserCollider.offset = new Vector2(0, phaserCollider.size.y / 2);
        }

        if (this.gameObject.layer == 22)
        {
            lineRenderer.material.color = UnityEngine.Random.ColorHSV(0.0f, 0.17f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0.0f, 0.17f, 1f, 1f, 0.1f, 0.5f));
        }
        else if (this.gameObject.layer == 23)
        {
            lineRenderer.material.color = UnityEngine.Random.ColorHSV(0.17f, 0.3f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0.17f, 0.4f, 1f, 1f, 0.1f, 0.5f));
        }
        else if (this.gameObject.layer == 24)
        {
            lineRenderer.material.color = UnityEngine.Random.ColorHSV(0.35f, 0.7f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0.35f, 0.7f, 1f, 1f, 0.1f, 0.5f));
        }
        else
        {
            lineRenderer.material.color = UnityEngine.Random.ColorHSV(0.0f, 0.1f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", UnityEngine.Random.ColorHSV(0.0f, 0.1f, 1f, 1f, 0.1f, 0.5f));
        }
    }


    private void subtractCost()
    {
        fuelController.currentFuel -= phaserCost;
        temperatureController.currentWeaponTemp += phaserWeaponTemp / 10;
    }

    public float getDamage(float length, float dmg)
    {
        phaserLengthPercent = 1 - (length/ maxRange);
        return (float)Math.Round(dmg * phaserLengthPercent, 2);
    }

    IEnumerator deactivatePhaser()
    {
        yield return new WaitForSeconds(0.5f);
        toggle();
    }

    public void toggle()
    {
        photonView.RPC("turnOff", RpcTarget.All);
    }

    [PunRPC]
    public void turnOff()
    {
        lineRenderer.enabled = false;
        phaserCollider.enabled = false; 
        Vector2 size = phaserCollider.size;
        size.y = 0;
        phaserCollider.size = size;
        phaserOn = false;
    }

    [PunRPC]
    public void firePhaserRPC(Vector2 p, float dmg)
    {
        RotateTowards(p);
        this.damage = dmg;
        lineRenderer.SetPosition(1, new Vector3(p.x, p.y, 0));
        lineRenderer.enabled = true;
        phaserCollider.enabled = true;
        phaserOn = true;
        Vector2 size = phaserCollider.size;
        size.y = new Vector2(transform.position.x - p.x, transform.position.y - p.y).magnitude;
        phaserCollider.size = size;
        phaserCollider.offset = new Vector2(0, phaserCollider.size.y / 2);
        StartCoroutine("deactivatePhaser");
    }

    void RotateTowards(Vector2 target)
    {
        float offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        this.transform.rotation = rotation;
    }
}
