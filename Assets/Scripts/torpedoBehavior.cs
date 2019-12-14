using System.Collections;
using UnityEngine;

public class TorpedoBehavior : MonoBehaviour
{
 
    [SerializeField]
    private float torpSpeed;
    [SerializeField]
    private float ttl;
    public PlayerController playerController;
    public int damage;

    private void OnEnable()
    {
        StartCoroutine(hide());      
    }

    void Update()
    {
        transform.position += transform.up * 1 * Time.deltaTime;
    }    

    public void detonate()
    {
        if (playerController != null)
        {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator hide()
    {
        yield return new WaitForSeconds(ttl);       
        this.gameObject.SetActive(false);
        if(playerController != null)
        {
            playerController.torps.Remove(this);
            playerController.torpCount--;
        }
    }
}
