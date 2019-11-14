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
    public float rechargeRate;
    public PlayerController playerController;
    [SerializeField]
    private TractorController tractorController;
    [SerializeField]
    private PressorController pressorController;
    public float shieldFuelCost;
    public float cloakFuelCost;    
    public float tractorFuelCost;
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
        }

        fuelPercent = (currentFuel/maxFuel);
    }

}
