using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torpedoBehavior : MonoBehaviour
{
 
    [SerializeField]
    private float torpSpeed;
    [SerializeField]
    private float ttl;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(hide());
      
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += transform.up * 1 * Time.deltaTime;

    }

    IEnumerator hide()
    {
        yield return new WaitForSeconds(ttl);       
        this.gameObject.SetActive(false);
    }
}
