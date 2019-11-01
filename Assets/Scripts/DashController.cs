using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class DashController : MonoBehaviour
{
 
    [SerializeField]
    private TMP_Text armys;
    [SerializeField]
    private TMP_Text weaponTemp;
    [SerializeField]
    private Image weaponImage;
    [SerializeField]
    private TMP_Text engineTemp;
    [SerializeField]
    private Image engineImage;
    [SerializeField]
    private TemperatureController temperatureController;
    [SerializeField]
    private TMP_Text fuel;
    [SerializeField]
    private Image fuelImage;
    [SerializeField]
    private FuelController fuelController;
    [SerializeField]
    private TMP_Text speed;
    [SerializeField]
    private TMP_Text shield;
    [SerializeField]
    private Image shieldImage;
    [SerializeField]
    private TMP_Text hull;
    [SerializeField]
    private Image hullImage;
    [SerializeField]
    private HullController hullController;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {        
        hull.text = string.Format("Hu[{0}/{1}]",  Math.Round(hullController.hullHealth, 0), hullController.hullMaxHealth);
        hullImage.fillAmount = hullController.hullPercent;

        shield.text = string.Format("Sh[{0}/{1}]", Math.Round(hullController.shieldHealth, 0), hullController.shieldMaxHealth);
        shieldImage.fillAmount = hullController.shieldPercent;

        fuel.text = string.Format("Fu[{0}/{1}]", Math.Round(fuelController.currentFuel, 0), fuelController.maxFuel);
        fuelImage.fillAmount = fuelController.fuelPercent;

        engineTemp.text = string.Format("Et[{0}/{1}]",Math.Round(temperatureController.currentEngineTemp, 0), temperatureController.maxEngineTemp);
        engineImage.fillAmount = temperatureController.engineTempPercent;

        weaponTemp.text = string.Format("Wt[{0}/{1}]", Math.Round(temperatureController.currentWeaponTemp, 0), temperatureController.maxWeaponTemp);
        weaponImage.fillAmount = temperatureController.weaponTempPercent;
    }
}
