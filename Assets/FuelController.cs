using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    [SerializeField]
    private float tickTimer;
    [SerializeField]
    private float nextTime;
    public float maxFuel;
    public float currentFuel;
    public float totalFuelCost;
    [SerializeField]
    private int rechargeRate;

    // Start is called before the first frame update
    void Start()
    {
        tickTimer = 0.1f;
        maxFuel = 10000;
        currentFuel = 8000;
    }

    // Update is called once per frame
    void Update()
    {               
        if (Time.time > nextTime )
        {
            nextTime = Time.time + tickTimer;
            currentFuel += rechargeRate;

            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            Debug.Log(currentFuel.ToString());

        }
        
    }

}
