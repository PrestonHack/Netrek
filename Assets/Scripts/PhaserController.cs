/*
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class PhaserController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float angle;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D phaserCollider;
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
    [SerializeField]
    private Vector2 end;
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


    private void Start()
    {
        soundClip = GetComponent<AudioSource>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        sizeCollider(end);
        damage = getDamage(lineRenderer.GetPosition(0), end, phaserDamage);

        if (Input.GetMouseButtonDown(0) && photonView.IsMine && fuelController.currentFuel >= phaserCost && Time.time > nextTime)
        {
            nextTime = Time.time + fireRate;
            mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            end = getEndPoint(lineRenderer.GetPosition(0), mousePosition, maxRange);
            photonView.RPC("firePhaserRPC", RpcTarget.AllBuffered, end, phaserDamage);
            subtractCost();
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

    private void FixedUpdate()
    {
        RotateTowards(end);
    }

    [PunRPC]
    public void firePhaserRPC(Vector2 endPoint, float dmg)
    {
        RotateTowards(endPoint);
        sizeCollider(endPoint);
        lineRenderer.SetPosition(1, new Vector3(endPoint.x, endPoint.y, 0));
        lineRenderer.enabled = true;
        phaserCollider.enabled = true;
        damage = getDamage(lineRenderer.GetPosition(0), endPoint, dmg);
        soundClip.enabled = true;
        StartCoroutine("deactivatePhaser");
    }

    IEnumerator deactivatePhaser()
    {
        yield return new WaitForSeconds(1);
        toggle();
    }
       
    private Vector2 getEndPoint(Vector2 begining, Vector2 ending, float range)
    {
        direction = ending - begining;
        distance = Mathf.Clamp(Vector2.Distance(begining, ending), 0, range);
        return begining + (direction.normalized * distance);
    }

    public void sizeCollider(Vector2 endPoint)
    {
        Vector2 size = phaserCollider.size;
        size.y = new Vector2(transform.position.x - endPoint.x, transform.position.y - endPoint.y).magnitude;
        phaserCollider.size = size;
        phaserCollider.offset = new Vector2(0, phaserCollider.size.y / 2);
    }

    public float getDamage(Vector2 beginning, Vector2 ending, float dmg)
    {
        length = ending - beginning;
        phaserLengthPercent = 1 - (length.magnitude / maxRange);
        return (float)Math.Round(dmg * phaserLengthPercent, 2);
    }

    public void toggle()
    {
        photonView.RPC("turnOff", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void turnOff()
    {
        lineRenderer.enabled = false;
        phaserCollider.enabled = false;
        soundClip.enabled = false;
        this.transform.rotation = Quaternion.identity;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collider check hit: " + collision.GetComponentInParent<Transform>().parent.name);
        photonView.RPC("moveLine",RpcTarget.AllBufferedViaServer, collision.transform.position);
    }

    [PunRPC]
    void moveLine(Vector3 pos)
    {
        end = pos;
    }
}
*/

/*
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class PhaserController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float angle;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D phaserCollider;
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
        soundClip = GetComponent<AudioSource>();
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(this.transform.root.gameObject.layer)) ^ LayerMask.GetMask("Fed", "Kli", "Ori", "Rom"); 
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        Vector2 size = phaserCollider.size;
        size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
        phaserCollider.size = size;
        phaserCollider.offset = new Vector2(0, phaserCollider.size.y / 2);

        if (Input.GetMouseButtonDown(0) && photonView.IsMine && fuelController.currentFuel >= phaserCost && Time.time > nextTime)
        {
        
                nextTime = Time.time + fireRate;
                mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                direction = mousePosition - new Vector2(transform.position.x, transform.position.y);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxRange, layerMask);

                if (hit.collider != null)
                {
                    end = hit.collider.transform.position;
                }
                else
                {
                    end = getEndPoint(lineRenderer.GetPosition(0), mousePosition, maxRange);
                }

                //photonView.RPC("sizeCollider",RpcTarget.All, end);

                photonView.RPC("firePhaserRPC", RpcTarget.All, end, phaserDamage);
                subtractCost();
      

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

    private void FixedUpdate()
    {
        RotateTowards(end);
    }

    [PunRPC]
    public void firePhaserRPC(Vector2 endPoint, float dmg)
    {
        RotateTowards(endPoint);
        lineRenderer.SetPosition(1, new Vector3(endPoint.x, endPoint.y, 0));
        lineRenderer.enabled = true;
        phaserCollider.enabled = true;
        damage = getDamage(lineRenderer.GetPosition(0), endPoint, dmg);
        soundClip.enabled = true;
        StartCoroutine("deactivatePhaser");
    }

    IEnumerator deactivatePhaser()
    {
        yield return new WaitForSeconds(1);
        toggle();
    }

    private Vector2 getEndPoint(Vector2 begining, Vector2 ending, float range)
    {
        direction = ending - begining;
        distance = Mathf.Clamp(Vector2.Distance(begining, ending), 0, range);
        return begining + (direction.normalized * distance);
    }

    public void sizeCollider(Vector2 endPoint)
    {
        Vector2 size = phaserCollider.size;
        size.y = new Vector2(transform.position.x - endPoint.x, transform.position.y - endPoint.y).magnitude;
        phaserCollider.size = size;
        phaserCollider.offset = new Vector2(0, phaserCollider.size.y / 2);
    }

    public float getDamage(Vector2 beginning, Vector2 ending, float dmg)
    {
        length = ending - beginning;
        phaserLengthPercent = 1 - (length.magnitude / maxRange);
        return (float)Math.Round(dmg * phaserLengthPercent, 2);
    }

    public void toggle()
    {
        photonView.RPC("turnOff", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void turnOff()
    {
        lineRenderer.enabled = false;
        phaserCollider.enabled = false;
        soundClip.enabled = false;
        this.transform.rotation = Quaternion.identity;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collider check hit: " + collision.GetComponentInParent<Transform>().parent.name);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    }
    
    void Update()
    {      

        if (Input.GetMouseButtonDown(0) && photonView.IsMine && fuelController.currentFuel >= phaserCost && Time.time > nextTime && !phaserOn)
        {
            if (photonView.IsMine)
            {
                nextTime = Time.time + fireRate;
                currentLength = 0;
                mousePosition = Input.mousePosition;
                point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
                start = new Vector2(transform.position.x, transform.position.y);
                direction = point - start;
                distance = Mathf.Clamp(Vector2.Distance(start, point), 0, maxRange);
                end = start + (direction.normalized * distance);
                phaserOn = true;
                photonView.RPC("firePhaserRPC", RpcTarget.All, end);
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
            RotateTowards(point);
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
    public void firePhaserRPC(Vector2 p)
    {
        RotateTowards(p);
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
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTriggerEnter2D_Phaser hit: " + col.transform.parent.root.name);
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
