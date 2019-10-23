using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client;

public class PlayerController : MonoBehaviourPunCallbacks
{
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
    private GameObject torp;
    [SerializeField]
    private Vector2 crosshairDebug;
    public GameObject weapon;
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private Vector3 navPosition;
    [SerializeField]
    private Vector3 point;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float playerSpeed;
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


    // Start is called before the first frame update
    void Start()
    {
        teamShipCanvas = GameObject.Find("TeamShipCanvas").GetComponent<Canvas>();
        navPoint = Vector2.up;
        pv = GetComponent<PhotonView>();       
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
        crosshairDebug = new Vector2(crosshairPoint.x , crosshairPoint.y);

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
        if(length < 0.1f)
        {
            navPoint = new Vector3(transform.position.x, transform.position.y,0) + transform.up;
            end = navPoint;
        }
        Debug.DrawLine(start, end, Color.yellow);       
        move(); 

        //torpedo code
        if (Input.GetKey(KeyCode.T) && Time.time > nextFire && !Input.GetKey(KeyCode.LeftShift))
        {
            nextFire = Time.time + fireRate;
            Vector3 weaponPosition = weapon.transform.position;
            Quaternion weaponRotation = weapon.transform.rotation;
            pv.RPC("fireTorp", RpcTarget.AllViaServer, weaponPosition,weaponRotation, this.gameObject.layer);

        }
        Debug.DrawLine(start, crosshairDebug);

        //shield code
        if (Input.GetKeyDown(KeyCode.U))
        {
            pv.RPC("activateShield", RpcTarget.AllBufferedViaServer);
        }


    }

    [PunRPC]
    public void activateShield()
    {
        if (shield.activeInHierarchy)
        {
            shield.SetActive(false);
            hullCollider.enabled = true;
        }
        else
        {
            shield.SetActive(true);
            hullCollider.enabled = false;
        }
    }


    [PunRPC]
    public void fireTorp(Vector3 position, Quaternion rotation, int layer)
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
            playerSpeed = 0.05f;
            rotationSpeed = 5f;
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            playerSpeed = 0.1f;
            rotationSpeed = 4f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            playerSpeed = 0.15f;
            rotationSpeed = 3f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            playerSpeed = 0.2f;
            rotationSpeed = 2f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            playerSpeed = 0.25f;
            rotationSpeed = 0.1f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            playerSpeed = 0.3f;
            rotationSpeed = 0.9f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            playerSpeed = 0.35f;
            rotationSpeed = 0.8f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            playerSpeed = 0.4f;
            rotationSpeed = 0.7f;

            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            playerSpeed = 0.45f;
            rotationSpeed = 0.6f;
           // audioPlayer.volume = 0.6f;
          //  audioPlayer.PlayOneShot(audioFiles[4]);
            return playerSpeed;
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            playerSpeed = 0;
         //   audioPlayer.volume = 0.4f;
         //   audioPlayer.PlayOneShot(audioFiles[5]);
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

        if (col.GetComponentInParent<Transform>().root.name != this.gameObject.GetComponentInParent<Transform>().root.name && col.gameObject.name == "tractor")
        {
            this.gameObject.transform.position -= (this.gameObject.transform.position - col.transform.position) * 0.0015f;


        }
        if (col.GetComponentInParent<Transform>().parent.name != this.gameObject.GetComponentInParent<Transform>().parent.name && col.gameObject.name == "pressor")
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
