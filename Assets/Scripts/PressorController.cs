using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PressorController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D pressorCollider;
    [SerializeField]
    private Transform parentTransform;
    private Vector3 mousePosition;
    public Vector2 point;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float rotationSpeed;
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
        Vector2 size = pressorCollider.size;
        size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
        pressorCollider.size = size;
        pressorCollider.offset = new Vector2(0, pressorCollider.size.y / 2);

        if (Input.GetKeyDown(KeyCode.Y))
        {          
            if (photonView.IsMine)
            {
                mousePosition = Input.mousePosition;
                point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
                photonView.RPC("firePressorRPC", RpcTarget.AllBuffered, point);
            }
        }
        lineRenderer.material.color = Random.ColorHSV(0.15f, 0.15f, 1f, 1f, 0.9f, 1f);
        lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.15f, 0.15f, 1f, 1f, 0.1f, 0.5f));
        lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));

    }
    private void FixedUpdate()
    {
        RotateTowards(point);
    }

    [PunRPC]
    public void firePressorRPC(Vector2 p)
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.SetPosition(1, new Vector3(p.x, p.y, 0));
            Vector2 size = pressorCollider.size;
            size.y = new Vector2(transform.position.x - p.x, transform.position.y - p.y).magnitude;
            pressorCollider.size = size;
            pressorCollider.offset = new Vector2(0, pressorCollider.size.y / 2);
            lineRenderer.enabled = true;
            pressorCollider.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
            pressorCollider.enabled = false;
        }
    }
    [PunRPC]
    public void moveBeam(Vector2 p)
    {
        point = p;
        lineRenderer.SetPosition(1, new Vector3(p.x, p.y, 0));
        Vector2 size = pressorCollider.size;
        size.y = new Vector2(transform.position.x - p.x, transform.position.y - p.y).magnitude;
        pressorCollider.size = size;
        pressorCollider.offset = new Vector2(0, pressorCollider.size.y / 2);

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D_Pressor");

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTriggerEnter2D_Pressor");
        Debug.Log(col.GetComponentInParent<Transform>().parent.name);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        
        if (col.GetComponentInParent<Transform>().root.name != this.gameObject.GetComponentInParent<Transform>().root.name)
        {
            parentTransform.parent.parent.transform.position -= (col.transform.position - parentTransform.parent.parent.transform.position).normalized * 0.0005f;
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
        //this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
}

