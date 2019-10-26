﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhaserController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D phaserCollider;
    [SerializeField]
    private Vector2 hit;
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
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 end;
    [SerializeField]
    public float distance;
    [SerializeField]
    private float maxRange = 1.75f ;
    
  
    void Update()
    {
        
        Vector2 size = phaserCollider.size;
        size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
        phaserCollider.size = size;
        phaserCollider.offset = new Vector2(0, phaserCollider.size.y / 2);
 
        if (Input.GetMouseButtonDown(0))
        {
            if (photonView.IsMine)
            {
                mousePosition = Input.mousePosition;
                point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
 
                start = new Vector2(transform.position.x, transform.position.y);
                direction = point - start;
                distance = Mathf.Clamp(Vector2.Distance(start, point), 0, maxRange);
                end = start + (direction.normalized * distance);
                photonView.RPC("firePhaserRPC", RpcTarget.AllBuffered, end);
            }
  
        }
        if(this.gameObject.layer == 10)
        {
            lineRenderer.material.color = Random.ColorHSV(0.0f, 0.17f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.0f, 0.17f, 1f, 1f, 0.1f, 0.5f));
        }
        else if(this.gameObject.layer == 11)
        {
            lineRenderer.material.color = Random.ColorHSV(0.17f, 0.3f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.17f, 0.4f, 1f, 1f, 0.1f, 0.5f));
        }
        else if (this.gameObject.layer == 12)
        {
            lineRenderer.material.color = Random.ColorHSV(0.35f, 0.7f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.35f, 0.7f, 1f, 1f, 0.1f, 0.5f));
        }
        else
        {
            lineRenderer.material.color = Random.ColorHSV(0.0f, 0.1f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.0f, 0.1f, 1f, 1f, 0.1f, 0.5f));
        }

        RotateTowards(point);
        lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));

    }

    [PunRPC]
    public void firePhaserRPC(Vector2 p)
    {
        lineRenderer.SetPosition(1, new Vector3(p.x, p.y, 0));            
        Vector2 size = phaserCollider.size;
        size.y = new Vector2(transform.position.x - p.x, transform.position.y - p.y).magnitude;
        phaserCollider.size = size;
        phaserCollider.offset = new Vector2(0, phaserCollider.size.y/2);        
        lineRenderer.enabled = true;
        phaserCollider.enabled = true;
        StartCoroutine("deactivatePhaser");
    }

    IEnumerator deactivatePhaser()
    {
        yield return new WaitForSeconds(1);
        lineRenderer.enabled = false;
        phaserCollider.enabled = false;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTriggerEnter2D");
        Debug.Log(col.GetComponentInParent<Transform>().parent.name);
    }

    void RotateTowards(Vector2 target)
    {
        float offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}

