using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    [SerializeField]
    private float tickTimer;
    [SerializeField]
    private float nextTime;
    public float fuelPercent;
    public float maxFuel;
    public float currentFuel;
    public float totalFuelCost;
    [SerializeField]
    private float rechargeRate;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private TractorController tractorController;
    [SerializeField]
    private PressorController pressorController;
    [SerializeField]
    private float shieldFuelCost;
    [SerializeField]
    private float cloakFuelCost;
    [SerializeField]
    private float tractorFuelCost;
    // Start is called before the first frame update
    void Start()
    {
        tickTimer = 0.1f;        
        currentFuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {               
        if (Time.time > nextTime )
        {
            nextTime = Time.time + tickTimer;
            currentFuel += (rechargeRate);
            currentFuel -= (playerController.warpFuelUse);
            if (playerController.shieldOn)
            {
                currentFuel -= (shieldFuelCost);
            }
            if (playerController.cloakOn)
            {
                currentFuel -= (cloakFuelCost);
            }
            if (tractorController.tractorOn)
            {
                currentFuel -= (tractorFuelCost);
            }
            if (pressorController.pressorOn)
            {
                currentFuel -= (tractorFuelCost);
            }
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            Debug.Log(currentFuel.ToString());

        }

        fuelPercent = (currentFuel/maxFuel);
    }

}
