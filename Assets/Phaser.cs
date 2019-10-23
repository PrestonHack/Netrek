using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Phaser : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int playerID;
    [SerializeField]
    private Vector3 hit;
    [SerializeField]
    private Collider2D phaserCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PhotonView.Find(playerID).GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        this.lineRenderer.SetPosition(0, new Vector3(playerController.weapon.transform.position.x, playerController.weapon.transform.position.y, playerController.weapon.transform.position.z));
        this.lineRenderer.SetPosition(1, new Vector3(hit.x, hit.y, hit.z));


    }
}
