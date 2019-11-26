﻿using UnityEngine;
using Photon.Pun;

public class TractorController : MonoBehaviour
{
    public bool tractorOn;
    [SerializeField]
    private EndPointController endPointController;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D tractorCollider;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private PhotonView photonView;
    public Vector2 point;
    public Vector2 direction;
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
    private Vector3 hardPoint;
    [SerializeField]
    private Vector2 hardPointCoords;
    [SerializeField]
    private float radians;


    void Update()
    {
        //setting hardpoint position manually
        radians = (endPointController.shipTransform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
        hardPoint = this.transform.position + endPointController.shipTransform.position;
        hardPointCoords.x = Mathf.Cos(radians);
        hardPointCoords.y = Mathf.Sin(radians);
        hardPoint = hardPointCoords * 0.1f;
        transform.position = endPointController.shipTransform.position + hardPoint;

        RotateTowards(endPointController.gameObject.transform.position);
        setCollider();
        lineRenderer.material.color = Random.ColorHSV(0.17f, 0.3f, 1f, 1f, 0.9f, 1f);
        lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.17f, 0.4f, 1f, 1f, 0.1f, 0.5f));
        lineRenderer.SetPosition(0, transform.position); 
        lineRenderer.SetPosition(1, endPointController.gameObject.transform.position);
    }

    public void off()
    {
        photonView.RPC("turnOff", RpcTarget.AllBuffered);
    }

    public void toggle()
    {
        photonView.RPC("enableTractor", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void turnOff()
    {
        lineRenderer.enabled = false;
        tractorCollider.enabled = false;
        tractorOn = false;
        endPointController.isOn = false;

    }

    [PunRPC]
    public void enableTractor()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            tractorCollider.enabled = true;
            tractorOn = true;
            endPointController.isOn = true;
        }
        else
        {
            lineRenderer.enabled = false;
            tractorCollider.enabled = false;
            tractorOn = false;
            endPointController.isOn = false;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Transform>().root.name != this.gameObject.GetComponentInParent<Transform>().root.name)
        {
            moveSelf(collision);
        }
    }

    private void moveSelf(Collider2D collision)
    {
        //transform.parent.parent.transform.position -= (transform.parent.parent.transform.position - collision.transform.position).normalized * 0.0005f;
        endPointController.end = collision.transform.position;
    }

    private void setCollider()
    {
        Vector2 size = tractorCollider.size;
        size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
        tractorCollider.size = size;
        tractorCollider.offset = new Vector2(0, tractorCollider.size.y / 2);
    }

    void RotateTowards(Vector2 target)
    {
        float offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime); ;
    }

}