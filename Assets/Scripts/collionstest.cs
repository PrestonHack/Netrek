using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collionstest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D_Test");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnCollisionEnter2D_Test");

    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnCollisionEnter2D_Test");

    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnCollisionEnter2D_Test");

    }
   
}
