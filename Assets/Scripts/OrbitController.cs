using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    [SerializeField]
    private float timePast;
    [SerializeField]
    private float orbitRadius;
    [SerializeField]
    private Vector2 orbitCoords;
    [SerializeField]
    private Vector2 orbitCenter;
    [SerializeField]
    private Quaternion orbitRotation;
    public bool orbiting;
    public bool orbitEnabled;
    [SerializeField]
    private PlayerController playerController;
    
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        timePast += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.O) && !orbiting && orbitEnabled)
        {
            orbiting = true;
        }
        else if (Input.GetKeyDown(KeyCode.O) && orbiting)
        {
            orbiting = false;
        }

        if (orbiting)
        {
            orbitCoords.x = Mathf.Cos(timePast * 0.4f);
            orbitCoords.y = Mathf.Sin(timePast * 0.4f);
            orbitCoords = orbitCoords * orbitRadius;
            orbitRotation = Quaternion.AngleAxis(-Mathf.Atan2(orbitCoords.x, orbitCoords.y) * Mathf.Rad2Deg + 90, Vector3.forward);
            transform.parent.parent.rotation = orbitRotation;
            transform.parent.parent.position = orbitCenter + orbitCoords;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerController.speedController.moveSpeed <= playerController.speedController.warpSpeed * 2)
        {
            orbitEnabled = true;
            orbitCenter = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        orbitEnabled = false;
    }

}
