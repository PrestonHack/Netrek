using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairController : MonoBehaviour
{
    [SerializeField]
    private float tickTimer;
    [SerializeField]
    private float nextTime;
    [SerializeField]
    private float repairRate;
    [SerializeField]
    private HullController hullController;
    [SerializeField]
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        tickTimer = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextTime)
        {
            nextTime = Time.time + tickTimer;
            if (playerController.repairOn)
            {
                hullController.shieldHealth += (repairRate/1000) * 4;
                hullController.hullHealth += (repairRate/1000) * 2;
            }
            else
            {
                if (playerController.shieldOn)
                {
                    hullController.shieldHealth += repairRate/1000;
                }
                else
                {
                    hullController.shieldHealth += (repairRate/1000) * 2;
                    hullController.hullHealth += repairRate/1000;
                }
            }         
            
        }
        
    }
}
