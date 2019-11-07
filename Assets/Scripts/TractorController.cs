using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TractorController : MonoBehaviour
{
    public bool tractorOn;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D tractorCollider;
    [SerializeField]
    private Transform parentTransform;
    private Vector3 mousePosition;
    public Vector2 point;
    [SerializeField]
    private Camera cam;
    private Vector2 direction;
    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 end;
    [SerializeField]
    public float distance;
    [SerializeField]
    public float currentLength;
    [SerializeField]
    private float maxRange = 2.25f;
    [SerializeField]
    private float angle;
    [SerializeField]
    private PhotonView photonView;

    private void Start()
    {
        point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
    }

    void Update()
    {
        Vector2 size = tractorCollider.size;
        size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
        tractorCollider.size = size;
        tractorCollider.offset = new Vector2(0, tractorCollider.size.y / 2);
        
        if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift))
        {
            if (photonView.IsMine)
            {
                currentLength = 0;
                mousePosition = Input.mousePosition;
                point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
                start = new Vector2(transform.position.x, transform.position.y);
                direction = point - start;
                distance = Mathf.Clamp(Vector2.Distance(start, point), 0, maxRange);
                end = start + (direction.normalized * distance);
                photonView.RPC("fireTractorRPC", RpcTarget.AllBuffered, end);
            }
        }
        if (photonView.IsMine)
        {
            currentLength = Vector2.Distance(transform.position, end);
            if (tractorOn && currentLength > maxRange)
            {
                photonView.RPC("turnOff", RpcTarget.All);
            }
        }
        lineRenderer.material.color = Random.ColorHSV(0.17f, 0.3f, 1f, 1f, 0.9f, 1f);
        lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.17f, 0.4f, 1f, 1f, 0.1f, 0.5f));
        lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));

    }
    private void FixedUpdate()
    {
        RotateTowards(point);
    }

    public void toggle()
    {
        photonView.RPC("turnOff", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void turnOff()
    {
        lineRenderer.enabled = false;
        tractorCollider.enabled = false;
        point = Vector2.one;
        tractorOn = false;
    }

    [PunRPC]
    public void fireTractorRPC(Vector2 p)
    {

        if (!lineRenderer.enabled)
        {
            lineRenderer.SetPosition(1, new Vector3(p.x, p.y, 0));
            Vector2 size = tractorCollider.size;
            size.y = new Vector2(transform.position.x - p.x, transform.position.y - p.y).magnitude;
            tractorCollider.size = size;
            tractorCollider.offset = new Vector2(0, tractorCollider.size.y / 2);
            lineRenderer.enabled = true;
            tractorCollider.enabled = true;
            tractorOn = true;
        }
        else
        {
            lineRenderer.enabled = false;
            tractorCollider.enabled = false;
            point = Vector2.one;
            tractorOn = false;
        }
    }

    [PunRPC]
    public void moveBeam(Vector2 p)
    {
        point = p;
        lineRenderer.SetPosition(1, new Vector3(p.x, p.y, 0));
        Vector2 size = tractorCollider.size;
        size.y = new Vector2(transform.position.x - p.x, transform.position.y - p.y).magnitude;
        tractorCollider.size = size;
        tractorCollider.offset = new Vector2(0, tractorCollider.size.y / 2);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTriggerEnter2D_Tractor");
        Debug.Log(col.GetComponentInParent<Transform>().parent.name);

    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponentInParent<Transform>().root.name != this.gameObject.GetComponentInParent<Transform>().root.name)
        {
            parentTransform.parent.parent.transform.position -= (parentTransform.parent.parent.transform.position - col.transform.position).normalized * 0.0005f;
            lineRenderer.SetPosition(1, col.transform.position);
            point = new Vector2(col.transform.position.x, col.transform.position.y);
            photonView.RPC("moveBeam", RpcTarget.AllBuffered, point);

        }
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
