using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] docks;
    [SerializeField]
    private CircleCollider2D dockCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.root.name + " is inside the docking collider: DockManager");
    }
}
