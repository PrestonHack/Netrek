using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockManager : MonoBehaviour
{
    public DockEndPoint[] docks;
    public Transform shipTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.root.name + " is inside the docking collider: DockManager");
    }

    private void Update()
    {
        this.gameObject.transform.position = shipTransform.position;
    }
}
