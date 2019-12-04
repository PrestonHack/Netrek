using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DashController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text killCountText;
    [SerializeField]
    private TMP_Text torpCountText;
    [SerializeField]
    private Image alertImage;
    [SerializeField]
    private Image lockFlagImage;
    [SerializeField]
    private Image repairFlagImage;
    [SerializeField]
    private Image shieldFlagImage;
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
    private PlayerController playerController;
    [SerializeField]
    private TMP_Text speed;
    [SerializeField]
    private Image speedImage;
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
    [SerializeField]
    private Color32 switchOnColor = new Color32(234, 84, 0, 200);
    [SerializeField]
    private Color32 switchOffColor = new Color32(255, 255, 255, 100);

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

        speed.text = string.Format("Sp[{0}/{1}]", playerController.warpNumber, playerController.maxWarp);
        speedImage.fillAmount = playerController.warpPercent;

        torpCountText.text = string.Format("Torps: {0}", playerController.torpCount.ToString());

        if (playerController.repairOn)
        {
            repairFlagImage.color = switchOnColor;
        }
        else
        {
            repairFlagImage.color = switchOffColor;
        }

        if (playerController.shieldOn)
        {
            shieldFlagImage.color = switchOnColor;
        }
        else
        {
            shieldFlagImage.color = switchOffColor;
        }

    }
}
