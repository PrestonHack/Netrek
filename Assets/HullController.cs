using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class HullController : MonoBehaviour
{
    [SerializeField]
    private float hullHealth;
    [SerializeField]
    private PolygonCollider2D hullCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("HullController_TriggerEnter: " + other.gameObject.name);
        
        if (Regex.IsMatch(other.gameObject.name, "@b\torp\b"))
        {
            Debug.Log("TORP!");
        }
        
    }

}
