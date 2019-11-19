using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockController : MonoBehaviour
{
    public bool docked;
    public bool dockingEnabled;
    [SerializeField]
    private DockManager dockManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.root.name + " is inside the docking collider: DockController");
        dockingEnabled = true;
        dockManager = collision.GetComponentInChildren<DockManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dockingEnabled = false;
    }
    private void Update()
    {
        if(dockingEnabled && !docked && Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Dock engaged");
            docked = true;
        }

        if(docked && Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Dock disengaged");
            docked = false;
        }
    }
}
