using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    [SerializeField]
    private float tickTimer;
    [SerializeField]
    private float nextTime;
    [SerializeField]
    private float tractorTemp;    
    public float maxWeaponTemp;
    public float maxEngineTemp;
    [SerializeField]
    private float engineCoolRate;
    [SerializeField]
    private float weaponCoolRate;
    [SerializeField]
    public float currentWeaponTemp;
    public float currentEngineTemp;
    public float weaponTempPercent;
    public float engineTempPercent;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private TractorController tractorController;
    [SerializeField]
    private PressorController pressorController;

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
            currentEngineTemp += playerController.warpNumber / 10;
            currentEngineTemp -= engineCoolRate / 10;

            currentWeaponTemp -= weaponCoolRate / 10;

            currentEngineTemp = Mathf.Clamp(currentEngineTemp, 0, maxEngineTemp);
            currentWeaponTemp = Mathf.Clamp(currentWeaponTemp, 0, maxWeaponTemp);

            if (tractorController.tractorOn)
            {
                currentEngineTemp += tractorTemp;
            }
            if (pressorController.pressorOn)
            {
                currentEngineTemp += tractorTemp;
            }
        }
        weaponTempPercent = (currentWeaponTemp / maxWeaponTemp);
        engineTempPercent = (currentEngineTemp / maxEngineTemp);
    }
}
